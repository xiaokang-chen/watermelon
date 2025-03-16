using System;
using System.Collections;
using System.Collections.Generic;
// using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using WeChatWASM;

public class VideoMax : MonoBehaviour
{
    private static VideoMax instance;

    public static VideoMax Instance
    {
        get
        {
            if (instance) return instance;

            var (con, count) = EventSystemExist();
            var obj = Resources.Load<VideoMax>("Preform/VideoMax");
            obj.gameObject.SetActive(false);
            var videoMax = Instantiate(obj);
            videoMax.name = obj.name;
            if (con && count >= 1)
            {
                videoMax.eventSystem.SetActive(false);
            }

            videoMax.showTime = obj.showTime;
            instance = videoMax;
            // DontDestroyOnLoad(instance.gameObject);
            instance.gameObject.SetActive(true);
            return instance;
        }
    }

    [Space(5)] public RawImage bg;
    public RawImage vi;

    public VideoPlayer videoPlayer;
    public Slider slider;
    [Space(5)] public Button playBtn;
    public Image PlayImage;
    public Sprite playimg;
    public Sprite stopimg;
    [Space(5)] public Button showReturn;
    [Space(5)] public Image victory;
    public Button victoryCancel;
    public Button victoryReturn;
    [Space(5)] public Canvas canvas;
    [Space(5)] public GameObject eventSystem;
    [Space(5)] public Button showSlider;
    public float showTime;
    private Action onVideoOver;
    private Action onVideoOver_back;
    private Action<Transform> onShowReturn;

    private bool isPlay;

    private bool IsPlay
    {
        get => isPlay;
        set
        {
            if (isPlay == value)
            {
                return;
            }

            isPlay = value;
            PlayImage.sprite = isPlay ? stopimg : playimg;
            if (isPlay)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
    }

    private bool isInit;

    private VideoMaxController _controller;
    private VideoData _videoData;

    private Action onInit;

    private VideoCharacteristic _videoCharacteristic = VideoCharacteristic.Default;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
    private Coroutine coroutine;

    private void Start()
    {
        IsPlay = true;
        _controller = new VideoMaxController(videoPlayer, slider, bg, vi);
        _controller.onVideoOver += VideoOver;
        _controller.onUpatePlayVideo += (video) => { coroutine = StartCoroutine(UpdatePlayVideo(video)); };
        _controller.IsPlay = IsPlay;
        _controller.showTime = this.showTime;
        playBtn.onClick.AddListener(() =>
        {
            IsPlay = !IsPlay;
            _controller.IsPlay = IsPlay;
        });

        showReturn.onClick.AddListener(() =>
        {
            if (onShowReturn != null)
            {
                onShowReturn.Invoke(canvas.transform);
                return;
            }

            victory.gameObject.SetActive(true);
        });

        victoryCancel.onClick.AddListener(() => { victory.gameObject.SetActive(false); });

        victoryReturn.onClick.AddListener(() =>
        {
            victory.gameObject.SetActive(false);
            Hide_Back();
        });

        showSlider.onClick.AddListener(() => { _controller.ShowSlider(); });

        SceneManager.sceneLoaded += CallBack;
        SceneManager.sceneUnloaded += _ => { eventSystem.SetActive(false); };

        onInit?.Invoke();
        isInit = true;
    }

    /// <summary>
    /// 设置视频并播放
    /// </summary>
    /// <param name="videoData">视频数据</param>
    /// <param name="replay">是否重新播放</param>
    /// <returns></returns>
    public VideoMax SetVideoData(VideoData videoData, bool replay = true)
    {
        if (videoPlayer == null)
        {
            Debug.Log("找不到视频数据");
            throw new NotImplementedException();
        }

        _videoData = videoData;
        Show();
        if (isInit)
        {
            SetVideoData(replay);
        }
        else
        {
            onInit += () => { SetVideoData(replay); };
        }

        IsPlay = true;
        onShowReturn = null;
        return this;
    }

    private void SetVideoData(bool replay = true)
    {
        //先判断是否是同一个视频
        if (_videoCharacteristic == _videoData.characteristic)
        {
            //是，重新 //否，继续
            _controller.SetVideo(_videoData.VideoUrl, false, replay);
        }
        else
        {
            //否，加载
            _controller.SetVideo(_videoData.VideoUrl);
            _videoCharacteristic = _videoData.characteristic;
        }
    }

    private void OnEnable()
    {
    }

    private void LateUpdate()
    {
        if (_controller is { isReady: true })
        {
            _controller.Update();
        }
    }


    public void Hide()
    {
        //停止视频
        Stop();
        onVideoOver?.Invoke();
        //隐藏
        gameObject.SetActive(false);
        //显示原本UI
        ShowAllUI();
    }

    private void Hide_Back()
    {
        //停止视频
        Stop();
        // onVideoOver_back?.Invoke();
        SceneManager.LoadScene("Start");
        //隐藏
        gameObject.SetActive(false);
        //显示原本UI
        ShowAllUI();
    }

    /// <summary>
    /// 播放结束
    /// </summary>
    /// <param name="action"></param>
    public void VideoOver(Action action)
    {
        onVideoOver = action;
    }

    /// <summary>
    /// 执行动作
    /// </summary>
    /// <param name="videoJumpAction">动作</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void PreformAction(VideoJumpAction videoJumpAction = VideoJumpAction.Default)
    {
        var ac = videoJumpAction == VideoJumpAction.Default ? _videoData.action : videoJumpAction;
        switch (ac)
        {
            case VideoJumpAction.Default:
                break;
            case VideoJumpAction.LastVideo:
                if (_videoData.lastVideo != VideoCharacteristic.Default)
                {
                    JumpScene();
                    SetVideoData(_videoData.lastVideo.GetVideoData());
                }
                else
                {
                    Debug.Log("上一个视频未指定");
                }

                break;
            case VideoJumpAction.NextVideo:
                if (_videoData.nextVideo != VideoCharacteristic.Default)
                {
                    JumpScene();
                    SetVideoData(_videoData.nextVideo.GetVideoData());
                }
                else
                {
                    Debug.Log("下一个视频未指定");
                }

                break;
            case VideoJumpAction.RepetitionVideo:
                SetVideoData(_videoData);
                break;
            case VideoJumpAction.DesignatedVideo:
                if (_videoData.designatedVideo != VideoCharacteristic.Default)
                {
                    JumpScene();
                    SetVideoData(_videoData.designatedVideo.GetVideoData());
                }
                else
                {
                    Debug.Log("未指定视频");
                }

                break;
            case VideoJumpAction.OverPlay:
                Hide();
                JumpScene();
                break;
            case VideoJumpAction.Wait:
                Pause();
                JumpScene();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// 点击返回按钮接口
    /// </summary>
    /// <param name="action"></param>
    public void UseTheCustomComponent(Action<Transform> action)
    {
        onShowReturn = action;
    }

    /// <summary>
    /// 播放速度0.1-10
    /// </summary>
    /// <param name="speed"></param>
    public void SetPlaySpeed(float speed)
    {
        if (isInit)
        {
            _controller?.SetPlaySpeed(speed);
        }
        else
        {
            onInit += () => { _controller?.SetPlaySpeed(speed); };
        }
    }

    /// <summary>
    /// 设置播放位置[0-1]
    /// </summary>
    /// <param name="location"></param>
    public void SetPlayLocation(float location)
    {
        if (isInit)
        {
            _controller?.SetPlayLocation(location);
        }
        else
        {
            onInit += () => { _controller?.SetPlayLocation(location); };
        }
    }

    private GameObject[] gameObjects;

    public VideoMax SetHideUI(params GameObject[] gameObjects)
    {
        this.gameObjects = gameObjects;
        if (gameObjects.Length <= 0)
        {
            return this;
        }

        foreach (var obj in gameObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.SetActive(false);
        }

        return this;
    }

    private void ShowAllUI()
    {
        if (gameObject == null || gameObjects.Length <= 0)
        {
            return;
        }

        foreach (var obj in gameObjects)
        {
            if (obj == null)
            {
                continue;
            }

            obj.SetActive(true);
        }
    }

    private void VideoOver()
    {
        onVideoOver?.Invoke();
        IsPlay = false;
        PreformAction();
    }

    private void CallBack(Scene scene, LoadSceneMode sceneType)
    {
        eventSystem.SetActive(false);
        var (con, count) = EventSystemExist();
        if (con && count > 1)
        {
            eventSystem.SetActive(false);
        }
        else if (count == 0)
        {
            eventSystem.SetActive(true);
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Play()
    {
        _controller?.PlayVideo();
    }

    private void Pause()
    {
        _controller?.PauseVideo();
    }

    private void Stop()
    {
        _controller?.StopVideo();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private static (bool, int) EventSystemExist()
    {
        var co = Camera.main.backgroundColor;
        co = Color.black;
        co.a = 0;
        Camera.main.backgroundColor = co;
        var eve = FindObjectsOfType<EventSystem>().Length;
        return (eve >= 1, eve);
    }

    private IEnumerator UpdatePlayVideo(WXVideo wxVideo)
    {
        while (true)
        {
            yield return waitForSeconds;
            if (!wxVideo.isPlaying)
            {
                wxVideo.Play();
            }
            else
            {
                Debug.Log("真正的播放");
                wxVideo.Play();
                StopCoroutine(coroutine);
                break;
            }
        }
    }

    private void JumpScene()
    {
        if (string.IsNullOrEmpty(_videoData.gameSceneName)) return;
        Hide();
        SceneManager.LoadScene(_videoData.gameSceneName);
    }
}