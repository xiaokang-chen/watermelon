using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WeChatWASM;


public class Test : MonoBehaviour
{
    public Button v1;
    public Button v2;

    public Button v3;

    public GameObject ui;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        v1.onClick.AddListener(
            () =>
            {
                VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene1.GetVideoData()).SetHideUI(ui);
            });
        v2.onClick.AddListener(
            () =>
            {
                /*VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene2.GetVideoData())
                    .UseTheCustomComponent(_ => { });*/
                VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene2.GetVideoData())
                    .SetHideUI(ui);
            });
        v3.onClick.AddListener(()=>{});
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene1.GetVideoData());
            VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene1.GetVideoData()).UseTheCustomComponent((transform1 => {Debug.Log("使用自定义面板");}));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            VideoMax.Instance.SetVideoData(VideoCharacteristic.VideoScene1.GetVideoData(), false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            VideoMax.Instance.SetPlayLocation(0.6f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}