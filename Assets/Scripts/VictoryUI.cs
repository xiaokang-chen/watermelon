using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    public bool isUsingTime = false;
    public TimeCountDown timeCountDown;
    public GameObject victoryUI;
    public string NextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isUsingTime && timeCountDown.CurTime <= 0){
            Time.timeScale = 0;
            victoryUI.SetActive(true);
        }
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // 继续当前场景
    public void ContinueToThisScene()
    {
        Time.timeScale = 1;
        victoryUI.SetActive(false);
    }

    // 加载下一个场景
    public void LoadNextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }
}
