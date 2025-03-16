using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Range(0, 40)] public int level = 1;

    public bool levelStatusFor17;
    public bool levelStatusFor15;
    public bool levelStatusFor16;
    public bool levelStatusFor40;
    public bool levelStatusFor39;
    public bool levelStatusFor38;

    public Image levelBar;
    public Image[] tenLevelImages;
    public Sprite tenLevelImageLocked;
    public Sprite tenLevelImageUnLocked;

    public List<Button> buttons;
    public Sprite LockedSprite;
    public Sprite[] UnLockedSprites;

    public Button btnBack;

    void Awake()
    {
        level = PlayerPrefs.GetInt("level", 0);
        levelStatusFor15 = PlayerPrefs.GetInt("levelStatusFor15", 0) == 1;
        levelStatusFor16 = PlayerPrefs.GetInt("levelStatusFor16", 0) == 1;
        levelStatusFor17 = PlayerPrefs.GetInt("levelStatusFor17", 0) == 1;
        btnBack.onClick.AddListener(OnBtnBack);
    }

    void Start()
    {
        int index = 0;
        foreach (Button button in buttons)
        {
            if (index > level && index != 17 && index != 15 && index != 16 && index != 40 && index != 39 && index != 38)
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }
            else if (index == 17 && !levelStatusFor17)
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }
            else if (index == 15 && !levelStatusFor15)
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }
            else if (index == 16 && !levelStatusFor16)
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }
            else if (index == 40 && !levelStatusFor40)
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }
            else if (index == 39 && !levelStatusFor39)
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }
            else if (index == 38 && !levelStatusFor38)
            {
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }
            else if (index <= level)
            {
                button.interactable = true;
                button.GetComponent<Image>().sprite = UnLockedSprites[index];
            }
            else{
                button.interactable = false;
                button.GetComponent<Image>().sprite = LockedSprite;
            }

            AddEventTrigger(button.gameObject, EventTriggerType.PointerUp, () => OnButtonClicked(button));
            index++;
        }

        levelBar.fillAmount = (float)level / 40;
        for (int i = 0; i < tenLevelImages.Length; i++)
        {
            if (levelBar.fillAmount >= (i + 1) / 10f)
            {
                tenLevelImages[i].sprite = tenLevelImageUnLocked;
            }
            else
            {
                tenLevelImages[i].sprite = tenLevelImageLocked;
            }
        }
    }

    void AddEventTrigger(GameObject obj, EventTriggerType type, UnityEngine.Events.UnityAction action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }

    void OnButtonClicked(Button button)
    {
        if(button.interactable == false)
        {
            return;
        }
        string sceneName = button.gameObject.name;
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private void OnBtnBack()
    {
        SceneManager.LoadScene("Start");
    }
}