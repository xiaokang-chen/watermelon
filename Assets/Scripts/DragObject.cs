using UnityEngine;
using UnityEngine.UI;

public class DragObject : MonoBehaviour
{
    public RectTransform draggableObject; // 可拖动的对象
    public float minX = -3888f; // X轴最小值
    public float maxX = 3634f; // X轴最大值
    private Vector3 lastMousePosition;

    void Update()
    {
        // 检测鼠标/手指的按下事件
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        // 检测鼠标/手指的拖动事件
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 newPosition = draggableObject.anchoredPosition + new Vector2(delta.x, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            draggableObject.anchoredPosition = newPosition;

            // 更新最后的鼠标位置
            lastMousePosition = Input.mousePosition;
        }
    }
}