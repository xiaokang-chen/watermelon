using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    string StartSceneName = "VideoScene 1.mp4";
    // Start is called before the first frame update
    void Start()
    {
       StartSceneName = PlayerPrefs.GetString("NowScene", "VideoScene 1.mp4");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClicked(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(StartSceneName);
    }
}
