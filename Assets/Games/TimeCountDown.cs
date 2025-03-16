using System.Collections;
using TMPro;
using UnityEngine;

public class TimeCountDown : MonoBehaviour
{
    public int time = 300;
    public TextMeshProUGUI timeText;
    private int curTime = 0;
    public int CurTime { get { return curTime; } }

    // Start is called before the first frame update
    void Start()
    {
        curTime = time;
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = curTime.ToString();
    }

    IEnumerator CountDown()
    {
        while (curTime > 0)
        {
            yield return new WaitForSeconds(1);
            curTime--;
        }
        curTime = 0;
    }

    public void ResetTime()
    {
        curTime = time;
        StartCoroutine(CountDown());
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
