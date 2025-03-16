using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Room2Controller : MonoBehaviour
{
    public Button BedSheetBtn;
    public Button ShengBtn;
    public Button WindowsBtn;
    public GameObject VictoryUI;
    public Button PlayAgainBtn;
    public Button LikaiBtn;
    // public GameObject DraggableBtn;
    public GameObject BedPhoto;
    public TextMeshProUGUI TypingText;
    public GameObject BlackScreen;
    public GameObject Target;
    public Camera mainCamera;

    public string NextSceneName;

    private bool isPhotoVisible = false;
    private bool isDraggableBtnVisible = false;
    private bool isTyping = false;

    public ObjectMover objectMover;
    public Transform objectMoveTarget;

    public CanvasGroup windows;
    public CanvasGroup windows2;

    void Start()
    {
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);

        BedSheetBtn.onClick.AddListener(OnBedSheetBtnClick);
        ShengBtn.onClick.AddListener(OnShengBtnClick);
        WindowsBtn.onClick.AddListener(OnWindowsBtnClick);
        LikaiBtn.onClick.AddListener(() => StartCoroutine(FadeInWindows2()));
        PlayAgainBtn.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        // DraggableBtn.SetActive(false);
        BlackScreen.SetActive(false);
    }

    void Update()
    {
        // if (isDraggableBtnVisible)
        // {
        //     float distance = Vector3.Distance(DraggableBtn.transform.position, Target.transform.position);
        //     if (distance < 2)
        //     {
        //         StartCoroutine(FadeInBlackScreen());
        //         DraggableBtn.SetActive(false); // Hide the draggable button once the black screen starts fading in
        //         isDraggableBtnVisible = false;
        //     }
        // }
    }

    void OnWindowsBtnClick()
    {
        // VictoryUI.SetActive(true);
        // VictoryUI2.SetActive(true);
        objectMover.isUnderControl = true;
        // move objectMover to objectMoveTarget
        StartCoroutine(MoveToPosition(objectMoveTarget.position));
    }

    private IEnumerator FadeInWindows()
    {
        windows.gameObject.SetActive(true);
        CanvasGroup canvasGroup = windows.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;

        VictoryUI.SetActive(true);
    }

    private IEnumerator FadeInWindows2()
    {
        windows2.gameObject.SetActive(true);
        CanvasGroup canvasGroup = windows2.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        StartCoroutine(FadeInBlackScreen());
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Mathf.Abs((targetPosition - objectMover.transform.position).magnitude) > 0.1f)
        {
            objectMover.animator.enabled = true;
            objectMover.animator.speed = 1;
            objectMover.transform.position = Vector3.MoveTowards(objectMover.transform.position, targetPosition, objectMover.speed * Time.deltaTime);
            if(targetPosition.x < objectMover.transform.position.x)
            {
                objectMover.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                objectMover.GetComponent<SpriteRenderer>().flipX = false;
            }
            yield return null;
        }
        objectMover.animator.speed = 1;
        // fade in windows
        StartCoroutine(FadeInWindows());
    }

    void OnBedSheetBtnClick()
    {
        if (!isPhotoVisible && !ShengBtn.gameObject.activeSelf)
        {
            ShowPhoto(BedPhoto, "床单...够结实，应该能打结当绳索用");
            ShengBtn.gameObject.SetActive(true);
        }
    }

    void OnShengBtnClick()
    {
        if (!isPhotoVisible)
        {
            ShowPhoto(null, "绳索 或许可以用来逃离这里");
            WindowsBtn.gameObject.SetActive(true);
        }
    }

    void ShowPhoto(GameObject photo, string message)
    {
        if (photo != null)
        {
            photo.SetActive(true);
            isPhotoVisible = true;
            StartCoroutine(HidePhotoAndShowMessage(photo, message));
        }
        else
        {
            StartCoroutine(TypeText(message));
        }
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
        if (!isDraggableBtnVisible)
        {
            // DraggableBtn.SetActive(true);
            isDraggableBtnVisible = true;
        }
    }

    IEnumerator FadeInBlackScreen()
    {
        BlackScreen.SetActive(true);
        CanvasGroup canvasGroup = BlackScreen.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = BlackScreen.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        SceneManager.LoadScene(NextSceneName);
    }
}