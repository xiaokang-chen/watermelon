using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PasswordLock : MonoBehaviour
{
    public string correctPassword = "0756";

    public int inputErrorTimes = 3;

    public TextMeshProUGUI PasswordText;

    public Button watchAdButton;
    public Button watchAdButton1;
    public Button watchAdButton2;

    public GameObject Tips;
    public TextMeshProUGUI tipsText;

    public string NextSceneName;
    public GameObject exitDialog;

    private string inputPassword = "";

    private int curInputErrorTimes;

    private bool isCanInput = true;

    private void Awake()
    {
        watchAdButton.onClick.AddListener(OnBtnWatchAd);
        watchAdButton.gameObject.SetActive(false);

        watchAdButton1.onClick.AddListener(OnBtnWatchAd1);
        watchAdButton1.gameObject.SetActive(false);

        watchAdButton2.onClick.AddListener(OnBtnWatchAd2);
        watchAdButton2.gameObject.SetActive(false);

        Tips.SetActive(false);
        tipsText.text = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);

        curInputErrorTimes = inputErrorTimes;
        PasswordText.text = inputPassword;
    }
    private void OnEnable()
    {
        exitDialog.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        PasswordText.text = inputPassword;
    }

    public void EnterNumber(int number)
    {
        if (!isCanInput) return;
        if (number >= 0 && number <= 9)
        {
            if (correctPassword.Length > inputPassword.Length)
            {
                inputPassword += number.ToString();
            }
            if (correctPassword.Length == inputPassword.Length)
            {
                isCanInput = false;
                if (inputPassword.Equals(correctPassword))
                {
                    PasswordText.color = Color.green;
                }
                else
                {
                    curInputErrorTimes--;
                    PasswordText.color = Color.red;
                    if(2 == curInputErrorTimes)
                    {
                        watchAdButton1.gameObject.SetActive(true);
                    }
                    else if(1 == curInputErrorTimes)
                    {
                        watchAdButton2.gameObject.SetActive(true);
                    }
                    if (0 == curInputErrorTimes)
                    {
                        watchAdButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        DOVirtual.DelayedCall(1, () =>
                        {
                            isCanInput = true;
                            PasswordText.color = Color.white;
                            inputPassword = "";
                        });
                    }
                }
            }
        }
    }

    public void ClearLastNumber()
    {
        if (inputPassword.Length > 0)
            inputPassword = inputPassword[..^1];
    }

    public void ConformPassword()
    {
        if (inputPassword == "0756")
        {
            Debug.Log("Password Correct!");
            SceneManager.LoadScene(NextSceneName);
        }
        else
        {
            Debug.Log("Password Incorrect!");
        }
    }

    private void OnBtnWatchAd()
    {
        WatchAd(() =>
        {
            isCanInput = true;
            inputPassword = correctPassword;
            watchAdButton.gameObject.SetActive(false);
            PasswordText.color = Color.green;
            DOVirtual.DelayedCall(2, () => { SceneManager.LoadScene(NextSceneName); });
        });
    }

    private void OnBtnWatchAd1()
    {
        WatchAd(() =>
        {
            Tips.SetActive(true);
            tipsText.text = "关于赵明轩父母";
        });
    }

    private void OnBtnWatchAd2()
    {
        WatchAd(() =>
        {
            Tips.SetActive(true);
            tipsText.text = "请到“回忆录”处翻看第十一集剧情";
        });
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
        SceneManager.LoadScene("Menu");
    }
}
