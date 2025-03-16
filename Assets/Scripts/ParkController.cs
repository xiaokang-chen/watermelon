using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ParkController : MonoBehaviour
{
    public int GameState = 0;

    public Button GrassBtn;
    public Button WallBtn;
    public Button WindowBtn;
    public Button NewGrassBtn;
    public Button NewWallBtn;
    public Button NewWindowBtn;
    public GameObject FootprintPhoto;
    public GameObject ScrewPhoto;
    public GameObject BloodPhoto;
    public TextMeshProUGUI TypingText;

    private bool isPhotoVisible = false;
    private bool isFirstGrassClick = true;
    private bool isFirstWallClick = true;
    private bool isFirstWindowClick = true;
    private GameObject currentPhoto;

    public Button boyItemButton1;
    public Button boyItemButton2;
    public GameObject boy1;
    public GameObject boy2;
    public Animator boy1Animator;
    public Animator boy2Animator;

    public CanvasGroup blackScreen;

    void Start()
    {
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);

        GrassBtn.onClick.AddListener(OnGrassBtnClick);
        WallBtn.onClick.AddListener(OnWallBtnClick);
        WindowBtn.onClick.AddListener(OnWindowBtnClick);

        NewGrassBtn.onClick.AddListener(OnGrassBtnClick);
        NewWallBtn.onClick.AddListener(OnWallBtnClick);
        NewWindowBtn.onClick.AddListener(OnWindowBtnClick);

        // Initially hide new buttons
        NewGrassBtn.gameObject.SetActive(false);
        NewWallBtn.gameObject.SetActive(false);
        NewWindowBtn.gameObject.SetActive(false);

        boyItemButton1.onClick.AddListener(OnBoyItemButton1Clicked);
        boyItemButton2.onClick.AddListener(OnBoyItemButton2Clicked);

        boy1.SetActive(true);
        boy2.SetActive(false);
    }

    void Update()
    {
        if(GameState == 4 && TypingText.text == ""){
            string message = "走吧，东西找⻬了，去找明轩";
            StartCoroutine(TypeText(message));
            GameState = 5;
        }
        else if(GameState == 6)
        {
            blackScreen.gameObject.SetActive(true);
            blackScreen.alpha += Time.deltaTime;
            if(blackScreen.alpha >= 1)
            {
                StartCoroutine(WaitForTwoSecond());
            }
        }
    }

    IEnumerator WaitForTwoSecond()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Puzzle_1_3");
    }

    void OnBoyItemButton1Clicked()
    {
        if(GameState != 1)
            return;
        GameState = 2;

        boy1.SetActive(true);
        boy2.SetActive(false);
        boy1Animator.Play("Boy1Animation");
    }

    void OnBoyItemButton2Clicked()
    {
        if(GameState != 5)
            return;

        boy1Animator.Play("Boy1Animation", -1, 0f);
        boy1.SetActive(false);
        boy2.SetActive(true);
        boy2Animator.Play("Boy2Animation");
        StartCoroutine(WaitForGameState5());
    }

    IEnumerator WaitForGameState5()
    {
        yield return new WaitForSeconds(3f);
        GameState = 6;
    }

    void OnGrassBtnClick()
    {
        if(GameState != 2 && GameState != 3 && GameState != 4)
            return;

        if (!isPhotoVisible)
        {
            currentPhoto = FootprintPhoto;
            ShowPhoto(currentPhoto);
            string message = isFirstGrassClick ? "找到了一顶安全帽，是给娃娃遮秃头的吗..." : "找到了⼀顶安全帽，是给娃娃遮秃头的吗...";
            StartCoroutine(TypeText(message));
            if (isFirstGrassClick)
            {
                if(GameState == 2)
                    GameState = 3;
                else if(GameState == 3)
                    GameState = 4;
        
                ReplaceButtons(GrassBtn, NewGrassBtn);
                isFirstGrassClick = false;
            }
        }
    }

    void OnWallBtnClick()
    {
        if(GameState != 2 && GameState != 3 && GameState != 4)
            return;

        if (!isPhotoVisible)
        {
            currentPhoto = ScrewPhoto;
            ShowPhoto(currentPhoto);
            string message = isFirstWallClick ? "找到了一副防护眼镜..." : "找到了⼀副防护眼镜...";
            StartCoroutine(TypeText(message));
            if (isFirstWallClick)
            {
                if(GameState == 2)
                    GameState = 3;
                else if(GameState == 3)
                    GameState = 4;

                ReplaceButtons(WallBtn, NewWallBtn);
                isFirstWallClick = false;
            }
        }
    }

    void OnWindowBtnClick()
    {
        if(GameState == 0)
        {
            GameState = 1;
        }

        if (!isPhotoVisible)
        {
            currentPhoto = BloodPhoto;
            ShowPhoto(currentPhoto);
            string message = isFirstWindowClick ? "不知道CG是什么的秋千..." : "不知道CG是什么的秋千...";
            StartCoroutine(TypeText(message));
            if (isFirstWindowClick)
            {
                ReplaceButtons(WindowBtn, NewWindowBtn);
                isFirstWindowClick = false;
            }
        }
    }

    void ReplaceButtons(Button oldButton, Button newButton)
    {
        oldButton.interactable = false;
        newButton.gameObject.SetActive(true);
    }

    void ShowPhoto(GameObject photo)
    {
        photo.SetActive(true);
        isPhotoVisible = true;
    }

    IEnumerator TypeText(string message)
    {
        TypingText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            TypingText.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust typing speed as needed
        }

        yield return new WaitForSeconds(2f); // Wait for a moment after typing
        StartCoroutine(WaitForScreenClick());
    }

    IEnumerator WaitForScreenClick()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        HidePhoto(currentPhoto);
        TypingText.text = "";
    }

    void HidePhoto(GameObject photo)
    {
        photo.SetActive(false);
        isPhotoVisible = false;
    }
}