using UnityEngine;

public class SpriteScreenAdapter : MonoBehaviour
{
    private void Start()
    {
        AdaptSpriteToScreen();
    }

    void AdaptSpriteToScreen()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        // 获取Sprite的宽高
        float spriteHeight = spriteRenderer.bounds.size.y / transform.localScale.y;

        // 获取摄像机的高度
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;

        // 计算缩放比例
        float scale = worldScreenHeight / spriteHeight;

        // 应用缩放
        transform.localScale = new Vector3(scale, scale, 1);
    }
}