using DG.Tweening;
using UnityEngine;

public class UIGameInstaceB : MonoBehaviour
{
    [Tooltip("对话次数")]
    public int conversations = 20;
    public SpineRoleComp spineRole;
    public GameObject adsBtnGameObject;
    public GameObject adsTipLabGameObject;

    public GameObject exitDialog;

    private void Awake()
    {
        spineRole.DialogueTTSCompete += () =>{
            DataStorageMgr.Instance.GameInstaneBTimes++;
        };
        spineRole.DialogueCompete += ()=>{
            if (DataStorageMgr.Instance.GameInstaneBTimes >= conversations) {
                UpdateAdsView(true);
            }
        };
    }

    private void OnEnable() {
        exitDialog.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DataStorageMgr.Instance.GameInstaneBTimes >= conversations)
        {
            UpdateAdsView(true);
        } else
        {
            UpdateAdsView(false);
        }
    }

    // Update is called once per frame
    // void Update() {}

    private void UpdateAdsView(bool visible) {
        adsTipLabGameObject.SetActive(visible);
        adsBtnGameObject.SetActive(visible);

        spineRole.inputField.gameObject.SetActive(!visible);
        spineRole.aiText.gameObject.SetActive(!visible);
    }

    public void OnBtnBack()
    {
        exitDialog.SetActive(true);
    }

    public void OnBtnWatchAds()
    {
        DataStorageMgr.Instance.GameInstaneBTimes = 0;
        UpdateAdsView(false);
    }

    public void PopIn() {
        gameObject.SetActive(true);

        transform.localPosition = new Vector3(Screen.width , 0);

        transform.DOLocalMoveX(0, 0.2F)
                 .SetEase(Ease.Linear);
    }

    public void PopOut()
    {
        transform.DOLocalMoveX(Screen.width, 0.2F)
                 .OnComplete(() => {
                    gameObject.SetActive(false);
                 });
    }
}
