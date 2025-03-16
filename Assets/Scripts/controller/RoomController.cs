using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    public bool isInRoom = false;
    public Button FloorBtn;
    public Button NewBtn;
    public Button BookshelfBtn;
    public Button BookshelfNewBtn;
    public Button CarpetBtn;
    public GameObject ScrewPhoto;
    public GameObject FootprintPhoto;
    public GameObject BookshelfPhoto;
    public TextMeshProUGUI TypingText;
    public SpriteRenderer BookshelfImage;
    public Sprite SpriteA;
    public Sprite SpriteB;
    public GameObject object1;
    public GameObject object2;

    private bool isPhotoVisible = false;
    private bool isFirstFloorClick = true;
    private bool isFirstBookshelfClick = true;

    private bool isCarpetClicked = false;
    private bool isFloorClicked = false;

    private bool isTyping = false;
    private GameObject currentPhoto;

    void Start()
    {
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);

        FloorBtn.onClick.AddListener(OnFloorBtnClick);
        NewBtn.onClick.AddListener(OnNewBtnClick);
        BookshelfBtn.onClick.AddListener(OnBookshelfBtnClick);
        BookshelfNewBtn.onClick.AddListener(OnBookshelfNewBtnClick);
        CarpetBtn.onClick.AddListener(OnCarpetBtnClick);

        NewBtn.gameObject.SetActive(false);
        BookshelfBtn.gameObject.SetActive(false);
        BookshelfNewBtn.gameObject.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(object1.transform.position, object2.transform.position);
        if (distance < 4 && isFloorClicked && isCarpetClicked)
        {
            BookshelfBtn.gameObject.SetActive(true);
            BookshelfImage.sprite = SpriteB;
        }
        else
        {
            BookshelfBtn.gameObject.SetActive(false);
            BookshelfImage.sprite = SpriteA;
        }
    }

    void OnFloorBtnClick()
    {
        if (!isPhotoVisible)
        {
            currentPhoto = ScrewPhoto;
            ShowPhoto(currentPhoto, "这些螺丝钉是干什么用的");
            isFloorClicked = true;
        }
    }

    void OnNewBtnClick()
    {
        if (!isPhotoVisible)
        {
            currentPhoto = FootprintPhoto;
            ShowPhoto(currentPhoto, "脚印，看起来像是四十码的鞋子");
        }
    }

    void OnBookshelfBtnClick()
    {
        if (!isPhotoVisible)
        {
            currentPhoto = BookshelfPhoto;
            ShowPhoto(currentPhoto, "有人带走了什么东西，是那本日记吗...");
            if (isFirstBookshelfClick)
            {
                BookshelfNewBtn.gameObject.SetActive(true);
                isFirstBookshelfClick = false;
            }
        }
    }

    void OnBookshelfNewBtnClick()
    {
        // Bookshelf new button functionality, add as needed
    }

    void OnCarpetBtnClick()
    {
        if (!isPhotoVisible)
        {
            isCarpetClicked = true;
            StartCoroutine(TypeText("该死...搜得真干净，地毯都不放过"));
        }
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

        if (!NewBtn.gameObject.activeSelf)
        {
            NewBtn.gameObject.SetActive(true);
        }
    }
}