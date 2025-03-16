using UnityEngine;

public class SafeAreaAdjuster : MonoBehaviour
{
    private RectTransform safeAreaTransform;

    void Start()
    {
        safeAreaTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // 转换屏幕坐标到锚点坐标
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // 设置锚点
        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;
    }

    void OnRectTransformDimensionsChange()
    {
        //ApplySafeArea();
    }
}
