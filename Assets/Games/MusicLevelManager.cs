using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicLevelManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public Image TipImage;
    public Sprite tipSprite;
    public Sprite missSprite;
    public Sprite hitSprite;
    public Sprite perfectSprite;
    public Sprite goodSprite;
    public string NextSceneName;
    public AudioSource audioSource;

    public CanvasGroup subtitle1;
    public CanvasGroup subtitle2;
    public CanvasGroup subtitle3;
    public CanvasGroup subtitle4;
    public CanvasGroup subtitle5;
    private float fadeDuration = 0.6f;

    private int lastScore = 0;

    public Image BloodBar;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("NowScene", SceneManager.GetActiveScene().name);
        
        TipImage.gameObject.SetActive(false);
        StartCoroutine(ShowSubtitles());
    }

    // Update is called once per frame
    void Update()
    {
        if(BloodBar.fillAmount < 0.01){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (TipImage.sprite != tipSprite || lastScore != score)
        {
            TipImage.sprite = tipSprite;
            lastScore = score;
            // shake the image
            StartCoroutine(ShakeImage());
        }

        if (!audioSource.isPlaying)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(NextSceneName);
        }

        scoreText.text = score.ToString();
    }

    IEnumerator ShakeImage()
    {
        TipImage.gameObject.SetActive(true);
        Vector3 originalPos = TipImage.transform.position;
        for (int i = 0; i < 10; i++)
        {
            TipImage.transform.position = originalPos + new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), 0);
            yield return new WaitForSeconds(0.05f);
        }
        TipImage.transform.position = originalPos;
        TipImage.gameObject.SetActive(false);
    }

    IEnumerator ShowSubtitles()
    {
        yield return StartCoroutine(FadeCanvasGroup(subtitle1, 0, 1, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(FadeCanvasGroup(subtitle1, 1, 0, fadeDuration));

        yield return StartCoroutine(FadeCanvasGroup(subtitle2, 0, 1, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(FadeCanvasGroup(subtitle2, 1, 0, fadeDuration));

        yield return StartCoroutine(FadeCanvasGroup(subtitle3, 0, 1, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(FadeCanvasGroup(subtitle3, 1, 0, fadeDuration));

        yield return StartCoroutine(FadeCanvasGroup(subtitle4, 0, 1, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(FadeCanvasGroup(subtitle4, 1, 0, fadeDuration));

        yield return StartCoroutine(FadeCanvasGroup(subtitle5, 0, 1, fadeDuration));
        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(FadeCanvasGroup(subtitle5, 1, 0, fadeDuration));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}