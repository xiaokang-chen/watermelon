using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIOldWang : MonoBehaviour
{
    public int score = 50;
    public SpineRoleComp roleComp;
    public GameObject victoryObj;
    public GameObject loseObj;
    public Text scoreLab;

    public string NextSceneName;
    public GameObject exitDialog;

    private TimeCountDown timeCountDown;
    private int curScore;

    private const string skipNextWord = "好吧，你赢了，我走，看你如何管理好项目";

    private void Awake()
    {
        timeCountDown = gameObject.GetComponent<TimeCountDown>();
        roleComp.PromptComplete += PromptComplete;
        victoryObj.SetActive(false);
        loseObj.SetActive(false);
        curScore = score;
        scoreLab.text = curScore.ToString();
    }
    // Start is called before the first frame update
    // void Start(){}
    private void OnEnable()
    {
        exitDialog.SetActive(false);
    }
    private void PromptComplete(PromptOut prompt)
    {
        int tmpScore = curScore;
        string patternNegative = @"-?\d+分"; // Matches numbers, negative or positive
        Regex regexNegative = new(patternNegative);
        Match matchNegative = regexNegative.Match(prompt.text);
        if (matchNegative.Success && matchNegative.Value.StartsWith("-"))
        {
            if (int.TryParse(matchNegative.Value.Replace("分", ""), out int delta))
            {
                curScore += delta;
            }
        }
        else
        {
            string patternPositive = @"(?<!-)\d+分"; // Matches positive numbers only
            Regex regexPositive = new(patternPositive);
            Match matchPositive = regexPositive.Match(prompt.text);
            if (matchPositive.Success)
            {
                if (int.TryParse(matchPositive.Value.Replace("分", ""), out int delta))
                {
                    curScore += delta;
                }
            }
        }
        if (tmpScore != curScore)
        {
            if (0 >= curScore)
            {
                loseObj.SetActive(true);
            }
            else if (100 <= curScore)
            {
                victoryObj.SetActive(true);
            }
            scoreLab.text = curScore.ToString();
            Debug.Log("updated score: " + curScore);
        }

    }

    public void OnBtnWatchAdSkip()
    {
        roleComp.bgAudioSource.Pause();
        WatchAd(() =>
        {
            loseObj.SetActive(false);
            UIMaleRole maleRole = roleComp as UIMaleRole;
            maleRole.PlayTTSAnim(TextEmoticonsType.Angry, skipNextWord);
            maleRole.DialogueCompete += () =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(NextSceneName);
            };
        });
    }

    public void OnBtnWatchAdRetry()
    {
        roleComp.bgAudioSource.Pause();
        WatchAd(() =>
        {
            roleComp.bgAudioSource.Play();
            timeCountDown.ResetTime();
            Time.timeScale = 1;
            curScore = score;
            scoreLab.text = curScore.ToString();
            loseObj.SetActive(false);
        });
    }

    public void OnBtnNext()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(NextSceneName);
    }

    public void OnBtnRetry()
    {
        timeCountDown.ResetTime();
        Time.timeScale = 1;
        curScore = score;
        scoreLab.text = curScore.ToString();
        victoryObj.SetActive(false);
    }

    private void WatchAd(Action action)
    {
        // todo
        action?.Invoke();
    }

    public void BackToMenu()
    {
        exitDialog.SetActive(true);
    }

    public void OnBtnExit()
    {
        SceneManager.LoadScene("Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCountDown.CurTime <= 0)
        {
            Time.timeScale = 0;
            loseObj.SetActive(true);
        }
    }
}
