using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public string nowUrl;
    public string NextSceneName;

    private VideoPlayerController videoPlayerController;

    [Header("Option")]
    public bool isOption = false;
    private bool isWaitingForOption = false;
    private bool hasPlayed = false;

    public string SceneName1;
    public string SceneName2;

    public GameObject OptionCanvas;
    public GameObject forOptionA;
    public GameObject forOptionB;

    void Awake()
    {
        VideoCharacteristic.VideoScene1.GetVideoData().VideoUrl = nowUrl;
        VideoCharacteristic.VideoScene1.GetVideoData().gameSceneName = NextSceneName;
        if(isOption){
            VideoCharacteristic.VideoScene1.GetVideoData().action = VideoJumpAction.Wait;
        }
        else{
            VideoCharacteristic.VideoScene1.GetVideoData().action = VideoJumpAction.NextVideo;
        }
        // VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene1.GetVideoData());
        VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene1.GetVideoData()).VideoOver(()=>{OnVideoEndReached();});
        // CreateVideo();
    }

    // private void CreateVideo()
    // {
    //     GameObject obj = (GameObject)Resources.Load("Prefabs/VideoPlayer");
    //     GameObject videoObj = Instantiate(obj);
    //     videoObj.transform.localPosition = new Vector3();
    //     videoPlayerController = videoObj.GetComponent<VideoPlayerController>();
    //     var videoPlayer = videoPlayerController.videoPlayer;
    //     videoPlayer.loopPointReached += OnVideoEndReached;
    //     videoPlayer.errorReceived += OnErrorReceived;
    // }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex-3);
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);
        
        if(SceneManager.GetActiveScene().buildIndex-3 == 15){
            PlayerPrefs.SetInt("levelStatusFor15", 1);
        }
        else if(SceneManager.GetActiveScene().buildIndex-3 == 16){
            PlayerPrefs.SetInt("levelStatusFor16", 1);
        }
        else if(SceneManager.GetActiveScene().buildIndex-3 == 17){
            PlayerPrefs.SetInt("levelStatusFor17", 1);
        }
        else if(SceneManager.GetActiveScene().buildIndex-3 == 38){
            PlayerPrefs.SetInt("levelStatusFor38", 1);
        }
        else if(SceneManager.GetActiveScene().buildIndex-3 == 39){
            PlayerPrefs.SetInt("levelStatusFor39", 1);
        }
        else if(SceneManager.GetActiveScene().buildIndex-3 == 40){
            PlayerPrefs.SetInt("levelStatusFor40", 1);
        }
        // AutoPlayVideo();
    }

    void Update()
    {
        if(isWaitingForOption && !hasPlayed){
            if(!forOptionA.gameObject.activeSelf){
                SceneManager.LoadScene(SceneName1);
                hasPlayed = true;
            }
            else if(!forOptionB.gameObject.activeSelf){
                SceneManager.LoadScene(SceneName2);
                hasPlayed = true;
            }
        }
    }

    private void AutoPlayVideo()
    {
        videoPlayerController?.PlayVideo(nowUrl);
    }

    // 视频准备完成事件处理
    private void OnPrepareCompleted(VideoPlayer vp)
    {
        Debug.Log("Video is prepared and ready to play.");
        // 开始播放视频
    }

    private void OnVideoEndReached()
    {
        Debug.Log("Video playback completed.");
        if(isOption){
            OptionCanvas.SetActive(true);
            forOptionA.SetActive(true);
            forOptionB.SetActive(true);
            isWaitingForOption = true;
        }
        else{
            SceneManager.LoadScene(NextSceneName);
        }    
    }


    // 视频播放错误事件处理
    private void OnErrorReceived(VideoPlayer vp, string message)
    {
        Debug.LogError("Error received while playing video: " + message);
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
