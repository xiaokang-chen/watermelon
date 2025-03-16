using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlumberGameController : MonoBehaviour
{
    public GameObject VictoryUI;
    public GameObject VictoryToHide;
    public GameObject LoseUI;
    public GameObject[] pipe;
    public int[] pipeDirection;
    public bool[] isPipeSymmetry;

    public int[] correctDirection;

    public TextMeshProUGUI stepCountText;
    public int stepCount = 0;

    public string nextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        stepCountText.text = stepCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckWin(){
        for(int i = 0; i < pipe.Length; i++){
            if(pipeDirection[i] != correctDirection[i] && !isPipeSymmetry[i]){
                return false;
            }
            else if(isPipeSymmetry[i] && Mathf.Abs(pipeDirection[i] - correctDirection[i]) == 1){
                return false;
            }
        }
        return true;
    }

    public void RotatePipe(int index){
        stepCount--;
        stepCountText.text = stepCount.ToString();
        if(stepCount == 0){
            LoseUI.SetActive(true);
            Debug.Log("Lose");
            return;
        }

        pipeDirection[index] = (pipeDirection[index] + 1) % 4;
        pipe[index].transform.Rotate(0, 0, 90);

        if(CheckWin()){
            VictoryUI.SetActive(true);
            VictoryToHide.SetActive(false);
            Debug.Log("Win");
        }
    }

    public void ReloadScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void NextScene(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName){
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}
