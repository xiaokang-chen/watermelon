using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoVictoryUI : MonoBehaviour
{
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }
}
