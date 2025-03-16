using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using WeChatWASM;

public class VideoMaxController
{
    public readonly bool isReady;

    public bool isPlay;
    public bool IsPlay;

    public float showTime;

    public Action onVideoOver;

    public Action<WXVideo> onUpatePlayVideo;
    
    [DllImport("__Internal")]
    private static extern bool isWechat();

    [DllImport("__Internal")]
    private static extern void changeSpeed(float speed);

    private static bool IsWeChat
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return isWechat();
#else
            return false;
#endif
        }
    }


    private VideoPlayer _videoPlayer;
    private Slider _slider;
    private EventController _eventController;
    private float progress = -1;

    private GameObject slider;
    private float adTime;
    private bool isDown;

    private float length;

    private float Length
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            length = IsWeChat ? (float)_videoPlayer.length / 1000 : (float)_videoPlayer.length;
#else
            length = (float)_videoPlayer.length;
#endif
            return length;
        }
    }

    private float frameCount;

    private float FrameCount
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            frameCount = IsWeChat ? _videoPlayer.frameCount / 1000 : _videoPlayer.frameCount;
#else
            frameCount = _videoPlayer.frameCount;
#endif
            return frameCount;
        }
    }

    private WXVideo _wxVideo;

    public VideoMaxController(VideoPlayer videoPlayer, Slider slider, params RawImage[] rawImages)
    {
        _videoPlayer = videoPlayer;
        if (IsWeChat)
        {
            foreach (var t in rawImages)
            {
                var color = t.color;
                color.a = 0;
                t.color = color;
            }

            var windowInfo = WX.GetWindowInfo();
            _wxVideo = WX.CreateVideo(new WXCreateVideoParam
            {
                x = 0,
                y = 0,
                width = (int)windowInfo.screenWidth,
                height = (int)windowInfo.screenHeight,//736,
                src = "",
                poster = null,
                initialTime = 0,
                playbackRate = 1,
                live = false,
                objectFit = null,
                controls = false,
                showProgress = false,
                showProgressInControlMode = false,
                backgroundColor = null,
                autoplay = false,
                loop = false,
                muted = false,
                obeyMuteSwitch = false,
                enableProgressGesture = false,
                enablePlayGesture = false,
                showCenterPlayBtn = false,
                underGameView = true
            });
            // _wxVideo.RequestFullScreen(0);

            _wxVideo.OnError(() => { Debug.Log("视频出错"); });
            _wxVideo.OnPlay(() => { Debug.Log("视频开始播放"); });
            _wxVideo.OnTimeUpdate(videoTime =>
            {
                if (_slider.maxValue != videoTime.duration)
                {
                    Debug.Log("获取到视频长度");
                    _slider.maxValue = videoTime.duration;
                }

                _slider.value = videoTime.position;
            });
            _wxVideo.OnPause(() => { Debug.Log("视频暂停播放"); });
            _wxVideo.OnEnded(() =>
            {
                Debug.Log("视频结尾");
                StopVideo();
                progress = -1;
                onVideoOver?.Invoke();
            });
        }
        else
        {
            _videoPlayer.clockResyncOccurred += (v, d) => { Debug.Log("时钟同步"); };
            _videoPlayer.errorReceived += (v, err) => { Debug.Log("视频下载错误"); };
            _videoPlayer.frameReady += (v, l) => { Debug.Log("新帧准备就绪"); };
            _videoPlayer.loopPointReached += (v) =>
            {
                Debug.Log("结尾");
                PauseVideo();
                progress = -1;
                onVideoOver?.Invoke();
            };
            _videoPlayer.prepareCompleted += (v) =>
            {
                Debug.Log("准备完成");
                _slider.maxValue = Length;
                Debug.Log($"frameCount:{FrameCount}-frameRate:{_videoPlayer.frameRate}");
                PlayVideo();
            };
            _videoPlayer.started += (v) => { Debug.Log("视频开始播放"); };
        }

        this.slider = slider.transform.parent.gameObject;
        _slider = slider;
        _slider.minValue = 0;

        _eventController = _slider.gameObject.AddComponent<EventController>();
        _eventController.onDown += SliderDown;
        _eventController.onUp += SliderUp;
        _eventController.onDrag += SliderDrag;


        isReady = true;
    }

    private string ti;

    public void Update()
    {
        if (IsWeChat)
        {
            if (!slider.activeSelf) return;
            if (isDown) return;
            adTime += Time.deltaTime;
            if (!(adTime >= showTime)) return;
            slider.SetActive(false);
            adTime = 0;
        }
        else
        {
            if (!_videoPlayer || !isPlay) return;
            _slider.value = (float)_videoPlayer.time;

            if (!slider.activeSelf) return;
            if (isDown) return;
            adTime += Time.deltaTime;
            if (!(adTime >= showTime)) return;
            slider.SetActive(false);
            adTime = 0;
        }
    }

    public void SetPlaySpeed(float speed)
    {
        if (IsWeChat)
        {
            changeSpeed(Mathf.Clamp(speed, 0.5f, 1.5f));
        }
        else
        {
            _videoPlayer.playbackSpeed = Mathf.Clamp(speed, 0.1f, 10f);
        }
    }

    public void SetPlayLocation(float location)
    {
        if (IsWeChat)
        {
            isPlay = false;
            PauseVideo();
            _wxVideo.Seek((int)(_slider.maxValue * location));
            PlayVideo();
        }
        else
        {
            if (!_videoPlayer) return;
            isPlay = false;
            PauseVideo();
            _videoPlayer.time = Length * location;
            _slider.value = (float)_videoPlayer.time;
            PlayVideo();
        }
    }

    public void SetVideo(string url, bool isLoad = true, bool replay = true)
    {
        if (isLoad)
        {
            LoadVideo(url);
            return;
        }

        progress = replay ? -1 : _slider.value;

        Prepare();
    }

    public void PlayVideo()
    {
        if (IsWeChat)
        {
            isPlay = true;
            onUpatePlayVideo?.Invoke(_wxVideo);
            Debug.Log("调用了play");
            if (progress == -1) return;
            _wxVideo.Seek((int)progress);
            _slider.value = progress;
        }
        else
        {
            if (progress != -1)
            {
                _videoPlayer.frame = (long)(progress * _videoPlayer.frameRate);
                _slider.value = progress;
            }

            isPlay = true;
            _videoPlayer.Play();
        }
    }

    public void PauseVideo()
    {
        isPlay = false;
        if (IsWeChat)
        {
            _wxVideo.Pause();
        }
        else
        {
            _videoPlayer.Pause();
        }
    }

    public void StopVideo()
    {
        isPlay = false;
        if (IsWeChat)
        {
            _wxVideo.Stop();
            _slider.value = 0;
            _wxVideo.src = "";
        }
        else
        {
            _videoPlayer.Stop();
        }
    }

    public void ShowSlider()
    {
        adTime = 0;
        slider.SetActive(true);
    }

    private void SliderDown(PointerEventData eventData)
    {
        PauseVideo();
        if (IsWeChat)
        {
            _wxVideo.Seek((int)_slider.value);
        }
        else
        {
            _videoPlayer.frame = (long)(_slider.value * _videoPlayer.frameRate);
        }

        adTime = 0;
        isDown = true;
    }

    private void SliderUp(PointerEventData eventData)
    {
        if (IsPlay)
        {
            PlayVideo();
        }

        isDown = false;
    }

    public void SliderDrag(PointerEventData eventData)
    {
        if (IsWeChat)
        {
            _wxVideo.Seek((int)_slider.value);
        }
        else
        {
            _videoPlayer.frame = (long)(_slider.value * _videoPlayer.frameRate);
        }
    }

    private void LoadVideo(string url)
    {
        if (IsWeChat)
        {
            _wxVideo.src = url;
        }
        else
        {
            _videoPlayer.url = url;
        }

        Prepare();
    }

    private void Prepare()
    {
        Debug.Log("加载视频");
        if (IsWeChat)
        {
            PlayVideo();
        }
        else
        {
            _videoPlayer.Prepare();
        }
    }
}