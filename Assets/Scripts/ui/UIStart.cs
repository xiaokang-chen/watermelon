using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStart : MonoBehaviour
{
    public GameObject aiPrefab;
    public string gameInstaceAName;
    public string mainName;
    public AudioSource bgAudioSource;

    private UIGameInstaceB uiGameInstaceB;


    private void Awake()
    {
        GameObject prefabInstance = Instantiate(aiPrefab);

        prefabInstance.transform.position = new Vector3(0f, 0f, 0f);
        prefabInstance.transform.rotation = Quaternion.identity;
        prefabInstance.transform.localScale = Vector3.one;

        Canvas canvas = FindObjectOfType<Canvas>();

        //// 将加载的Prefab实例设为Canvas的子对象
        prefabInstance.transform.SetParent(canvas.transform, false);

        uiGameInstaceB = prefabInstance.GetComponent<UIGameInstaceB>();

        prefabInstance.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (uiGameInstaceB.gameObject.activeSelf)
        {
            if (bgAudioSource.isPlaying)
            {
                bgAudioSource.Pause();
            }
        }
        else {
            if (!bgAudioSource.isPlaying)
            {
                bgAudioSource.Play();
            }
        }
    }

    public void OnBtnGameInstanceB() {
        uiGameInstaceB.PopIn();
    }

    public void OnBtnGameInstanceA()
    {
       SceneManager.LoadScene(gameInstaceAName);
    }

    public void OnBtnMain()
    {
        SceneManager.LoadScene(mainName);
    }
}
