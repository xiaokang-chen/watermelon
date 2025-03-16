using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CourtyardController : MonoBehaviour
{
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
    private bool isTyping = false;

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
    }

    void OnGrassBtnClick()
    {
        if (!isPhotoVisible)
        {
            currentPhoto = FootprintPhoto;
            ShowPhoto(currentPhoto, isFirstGrassClick ? "窗边的草丛里有脚印，很可疑...拍个照吧" : "可疑的脚印照片，蹲在别人家窗子外面，多半是想干点什么吧...");
            if (isFirstGrassClick)
            {
                ReplaceButtons(GrassBtn, NewGrassBtn);
                isFirstGrassClick = false;
            }
        }
    }

    void OnWallBtnClick()
    {
        if (!isPhotoVisible)
        {
            currentPhoto = ScrewPhoto;
            ShowPhoto(currentPhoto, isFirstWallClick ? "窗户上的螺丝钉，为什么会掉在这里..." : "窗户上的螺丝钉，显然不是它自己掉下来的");
            if (isFirstWallClick)
            {
                ReplaceButtons(WallBtn, NewWallBtn);
                isFirstWallClick = false;
            }
        }
    }

    void OnWindowBtnClick()
    {
        if (!isPhotoVisible)
        {
            currentPhoto = BloodPhoto;
            ShowPhoto(currentPhoto, isFirstWindowClick ? "不知道这是谁的血，但一定是个该死的混蛋..." : "我会抓到你的...一定");
            if (isFirstWindowClick)
            {
                ReplaceButtons(WindowBtn, NewWindowBtn);
                isFirstWindowClick = false;
            }
        }
    }

    void ReplaceButtons(Button oldButton, Button newButton)
    {
        oldButton.gameObject.SetActive(false);
        newButton.gameObject.SetActive(true);
    }

    void ShowPhoto(GameObject photo, string message)
    {
        photo.SetActive(true);
        isPhotoVisible = true;
        StartCoroutine(HidePhotoAndShowMessage(photo, message));
    }

    IEnumerator HidePhotoAndShowMessage(GameObject photo, string message)
    {
        yield return new WaitForSeconds(2f); // Display photo for 2 seconds

        photo.SetActive(false);
        isPhotoVisible = false;

        yield return StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string message)
    {
        if (isTyping)
        {
            yield break;
        }
        isTyping = true;

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

        TypingText.text = "";
        isTyping = false;
    }

    void HidePhoto(GameObject photo)
    {
        photo.SetActive(false);
        isPhotoVisible = false;
    }
}