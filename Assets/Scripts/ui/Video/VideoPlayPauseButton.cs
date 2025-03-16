using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class VideoPlayPauseButton : MonoBehaviour
{
    public Sprite playTexture;
    public Sprite pauseTexture;

    private bool isPlay = true;

    public bool IsPlay 
    {
        set { 
            if (value != isPlay) {
                isPlay = value; 
                UpdateButtonState();
            }
        }
    }

    private Button button;

    public delegate void OnVideoStateChangeDelegate(bool isPlay);

    public event OnVideoStateChangeDelegate OnVideoStateChange;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnBtnClick);
        UpdateButtonState();
    }

    private void OnBtnClick()
    {
        isPlay = !isPlay;
        UpdateButtonState();
        OnVideoStateChange?.Invoke(isPlay);
    }

    private void UpdateButtonState() 
    {
        button.image.sprite = isPlay ? pauseTexture : playTexture;
    }
}
