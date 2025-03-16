using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PuzzleVictory : MonoBehaviour
{
    public bool isUsingMusic = false;
    public AudioSource audioSource;
    public Image[] Buttons;
    public Sprite[] RightSprites;

    public GameObject VictoryUI;

    public string NextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        bool isVictory = true;
        for(int i = 0; i < Buttons.Length; i++){
            if(Buttons[i].sprite != RightSprites[i]){
                isVictory = false;
                break;
            }
        }
        if(isVictory){
            if(isUsingMusic){
                audioSource.Play();
                isUsingMusic = false;
            }
            VictoryUI.SetActive(true);
        }
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextSceneName);
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
