using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WatermelonGameController : MonoBehaviour
{
    public GameObject victoryUI;
    public Transform line;
    // public GameObject loseUI;
    public AudioSource blastAudioSource;
    public AudioSource collisonAudioSource;
    public AudioSource clickAudioSource;
    public int GoalScore = 10000;
    public int Score = 0;
    public TextMeshProUGUI ScoreText;

    // 创建一个静态的实例变量
    public static WatermelonGameController Instance { get; private set; }

    public RectTransform StartYPosition;

    public GameObject rankPrefab;
    public Button btnRank;
    public Button btnBack;

    public GameObject exitDialog;


    private UIRanking uiRanking;

    private Rigidbody2D nowWatermelonRigidbody; // 用于缓存Rigidbody2D组件
    private GameObject lastWatermelon;

    private int combo = 0;

    public int Combo { set { combo = value; } get { return combo; } }

    // Awake在对象初始化时调用
    private void Awake()
    {

        Instance = this;
        btnRank.onClick.AddListener(OnBtnShowRank);
        btnBack.onClick.AddListener(OnBtnBack);
        victoryUI.SetActive(false);
    }

    private void Start()
    {
        DOVirtual.DelayedCall(0.2F, () => { CreateNowWatermelon(null); });
    }
    private void OnEnable()
    {
        exitDialog.SetActive(false);
    }
    //private float coolDownTimer = 0;
    //private bool isCoolDown = false;
    void Update()
    {
        // 添加日志来查看方法被调用
        Debug.Log("ApplyForce被调用");

        if (null != uiRanking && uiRanking.gameObject.activeSelf) return;
        if (victoryUI.activeSelf) return;
        if (exitDialog.activeSelf) return;
        if (Input.GetKeyUp(KeyCode.Mouse0) && nowWatermelonRigidbody != null)
        {
            clickAudioSource.Play();
            if (RectTransformUtility.RectangleContainsScreenPoint(btnRank.GetComponent<RectTransform>(), Input.mousePosition))
            {
                return;
            }
            if (RectTransformUtility.RectangleContainsScreenPoint(btnBack.GetComponent<RectTransform>(), Input.mousePosition))
            {
                return;
            }
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            worldPosition.y = nowWatermelonRigidbody.position.y;
            nowWatermelonRigidbody.position = worldPosition;
            nowWatermelonRigidbody.gravityScale = 1;
            //nowWatermelonRigidbody.AddForce(new Vector2(0, -5), ForceMode2D.Impulse);
            lastWatermelon = nowWatermelonRigidbody.gameObject;
            nowWatermelonRigidbody = null;
            combo = 0;
        }

        ScoreText.text = Score.ToString();

        if (Score >= GoalScore)
        {
            if (!victoryUI.activeSelf)
            {
                DataStorageMgr.Instance.WatermelonScore = Score;
                victoryUI.SetActive(true);
                ShowRank();
            }
        }
    }

    public void CreateNowWatermelon(GameObject collsionObj)
    {
        if (collsionObj != lastWatermelon) return;
        if (null != lastWatermelon)
        {
            var spriteRenderer = lastWatermelon.GetComponent<SpriteRenderer>();
            var maxPosY = line.position.y; // GetStartWorldPos().y;
            // convert y to world position
            maxPosY = Camera.main.ScreenToWorldPoint(new Vector3(0, maxPosY, Camera.main.nearClipPlane)).y;

            if (maxPosY <= spriteRenderer.bounds.max.y)
            {
                Debug.Log("lose");
                line.GetComponent<Image>().color = Color.red;
                DataStorageMgr.Instance.WatermelonScore = Score;
                victoryUI.SetActive(true);
                ShowRank();
                return;
            }
        }
        int levelPlus = Random.Range(0, 3);
        GameObject watermelon = WatermelonObjectPoolMgr.Instance.Get(WatermelonObjectType.WatermelonBegin + levelPlus);
        var worldPosition = GetStartWorldPos();
        var sp = watermelon.GetComponent<SpriteRenderer>();
        worldPosition.y += sp.bounds.size.y / 2;
        watermelon.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
        nowWatermelonRigidbody = watermelon.GetComponent<Rigidbody2D>();
        nowWatermelonRigidbody.gravityScale = 0;
        lastWatermelon = null;
    }

    private Vector3 GetStartWorldPos()
    {
        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(new Vector3(StartYPosition.position.x, StartYPosition.position.y, Camera.main.nearClipPlane));
        return screenPoint;
    }

    // 创建一个指定级别的西瓜在指定位置
    public GameObject CreateWatermelon(int level, Vector2 position)
    {
        if (level < WatermelonObjectPoolMgr.Instance.MaxWatermelonPrefabsCount)
        {
            GameObject watermelon = WatermelonObjectPoolMgr.Instance.Get(WatermelonObjectType.WatermelonBegin + level);
            watermelon.transform.SetPositionAndRotation(position, Quaternion.identity);
            watermelon.name = "Watermelon Level " + level;
            return watermelon;
        }
        Debug.Log("已达到最大级别，无法创建更高级别的西瓜");
        return null;
    }

    public void OnBtnShowRank()
    {
        clickAudioSource.Play();
        ShowRank();
    }

    private void ShowRank()
    {
        if (null == uiRanking)
        {
            var obj = Instantiate(rankPrefab);
            uiRanking = obj.GetComponent<UIRanking>();
        }
        uiRanking.PopIn();
    }

    public void PlayBlastAudioEffect()
    {
        blastAudioSource.Play();
    }

    public void PlayCollisionAudioEffect()
    {
        collisonAudioSource.Play();
    }

    private void OnBtnBack()
    {
        exitDialog.SetActive(true);
    }

    public void OnBtnExit()
    {
        SceneManager.LoadScene("Start");
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}