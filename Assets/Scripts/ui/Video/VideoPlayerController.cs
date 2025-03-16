using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public Button btnScreen;

    public Slider progressBar;
    public VideoPlayPauseButton btnPlay;
    public float hideDelay = 5f;

    private float lastInteractionTime;

    private bool isCheckHideProgressBar = true;

    // public TextMeshProUGUI timeText;

    private VideoSliderManager videoSliderManager;

    private bool isDrag = false;

    private double VideoTotalDuation
    {
        get
        {
#if UNITY_WEBGL
            if (Application.platform == RuntimePlatform.WebGLPlayer && WeChatDetector.IsWeChat())
            {
                return videoPlayer.length / 1000;
            }
#endif
            return videoPlayer.length;
        }
    }

    private ulong VideoFrameCount
    {
        get
        {
#if UNITY_WEBGL
            if (Application.platform == RuntimePlatform.WebGLPlayer && WeChatDetector.IsWeChat())
            {
                return videoPlayer.frameCount / 1000;
            }
#endif
            return videoPlayer.frameCount;
        }
    }

    private void Awake()
    {
        videoSliderManager = progressBar.GetComponent<VideoSliderManager>();
        videoSliderManager.OnDragListener += OnDrag;
        videoSliderManager.OnEndDragListener += OnEndDrag;
        btnPlay.OnVideoStateChange += OnVideoStateChange;
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.frameReady += (VideoPlayer source, long frameIdx)=>{
            Debug.Log("frameReady:" + frameIdx);
        };
        // videoPlayer.seekCompleted += OnSeekCompleted;
        btnScreen.onClick.AddListener(ShowControls);
    }

    // void Start() {}

    void Update()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            // 更新进度条
            progressBar.value = (float)(videoPlayer.time / VideoTotalDuation);
            // 更新时间文本
            /*string currentTime = FormatTime(videoPlayer.time);
            string totalTime = FormatTime(videoPlayer.length);
            timeText.text = $"{currentTime} / {totalTime}";*/
        }
        // 检查隐藏控件的时间
        if (isCheckHideProgressBar)
        {
            if (Time.time - lastInteractionTime > hideDelay)
            {
                HideControls();
            }
        }
    }

    public void PlayVideo(string url)
    {
        videoPlayer.url = url;
        btnPlay.IsPlay = true;
        videoPlayer.Prepare();
    }

    /*string FormatTime(double time)
    {
        int minutes = Mathf.FloorToInt((float)time / 60F);
        int seconds = Mathf.FloorToInt((float)time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }*/
    private void OnPrepareCompleted(VideoPlayer vp)
    {
        Debug.Log("Video is prepared and ready to play.");
        Debug.Log("frameRate: " + vp.frameRate);
        Debug.Log("frameCount: " + vp.frameCount);
        Debug.Log("video length: " + vp.length);
        // 开始播放视频
        vp.Play();
    }
    private void OnSeekCompleted(VideoPlayer vp)
    {
        Debug.Log("OnSeekCompleted.");
        // progressBar.maxValue = VideoTotalDuation;
        // 开始播放视频
        vp.Play();
    }
    private void OnDrag(PointerEventData eventData)
    {
        isCheckHideProgressBar = false;
        isDrag = true;
        videoPlayer.Pause();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCheckControlsAutoHide();
        OnProgressBarChanged();
        isDrag = false;
    }

    // 当滑动进度条时调用
    public void OnProgressBarChanged()
    {
        Debug.Log("progress: " + progressBar.value + " VideoTotalDuation " + VideoTotalDuation);
        if (videoPlayer != null)
        {
            // var time = progressBar.value * VideoTotalDuation;
            Seek(progressBar.value);
        }
        Debug.Log("OnProgressBarChanged time: " + videoPlayer.time);
    }

    private void OnVideoStateChange(bool isPlay)
    {
        if (isDrag)
        {
            Debug.Log("OnVideoStateChange isDrag");
            return;
        }
        if (isPlay)
        {
            if (videoPlayer.isPaused)
            {
                videoPlayer.Play();
                StartCheckControlsAutoHide();
            }
        }
        else
        {
            if (videoPlayer.isPlaying)
            {
                ShowControls();
                videoPlayer.Pause();
                isCheckHideProgressBar = false;
            }
        }
    }
    private void HideControls()
    {
        progressBar.transform.parent.gameObject.SetActive(false);
        isCheckHideProgressBar = false;
    }
    private void ShowControls()
    {
        Debug.Log("time: " + videoPlayer.time + " progress: " + progressBar.value);
        Debug.Log("frameCount: " + videoPlayer.frameCount);
        progressBar.transform.parent.gameObject.SetActive(true);
        StartCheckControlsAutoHide();
    }

    private void StartCheckControlsAutoHide()
    {
        isCheckHideProgressBar = true;
        lastInteractionTime = Time.time;
    }

    public void Seek(float seek)
    {
        // videoPlayer.time = seek;
        videoPlayer.frame = (long)(Mathf.Clamp01(seek) * VideoFrameCount);
        Debug.Log("Seek frame: " + videoPlayer.frame);
    }

    public void SeekBySeconds(double seconds)
    {
        double seek = seconds / VideoTotalDuation;
        Seek((float)seek);
    }
}