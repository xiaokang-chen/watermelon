using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClimbController : MonoBehaviour
{
    public bool isFinished = false; // 是否完成了攀爬
    public Animator animator;
    public CanvasGroup blackScreen; // UI CanvasGroup for black screen effect
    public float flashDuration = 0.1f; // Duration of the flash effect

    private bool isFlashing = false;

    // Start is called before the first frame update
    void Start()
    {
        // 确保 Animator 和 BlackScreen 已经被赋值
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (blackScreen != null)
        {
            blackScreen.alpha = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 检查鼠标左键是否被按下
        if (Input.GetMouseButton(0)) // 0 表示鼠标左键
        {
            // 播放动画
            animator.speed = 1.0f;
        }
        else
        {
            // 暂停动画
            animator.speed = 0.0f;
        }

        // 检查动画播放进度
        if (!isFinished && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f && !isFlashing)
        {
            isFinished = true;
            StartCoroutine(FlashBlackScreen());
        }
    }

    // 闪黑效果的协程
    private IEnumerator FlashBlackScreen()
    {
        isFlashing = true;

        // 暂停动画
        animator.speed = 0.0f;

        // 淡入黑屏
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            blackScreen.alpha = Mathf.Clamp01(elapsedTime / flashDuration);
            yield return null;
        }

        // 确保黑屏完全显示
        blackScreen.alpha = 1f;

        isFlashing = false;
    }
}