using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool isThree = false;
    private Vector3 startPosition;
    public List<Image> targetImages; // 目标Images的列表

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; // 拖动
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (var targetImage in targetImages)
        {
            // 遍历所有目标Image，检查拖动的Image是否在其区域内
            if (RectTransformUtility.RectangleContainsScreenPoint(targetImage.rectTransform, Input.mousePosition))
            {
                // 如果松开时鼠标在任一TargetImage内
                targetImage.sprite = GetComponent<Image>().sprite; // 更改Sprite
                targetImage.color = GetComponent<Image>().color; // 更改颜色
                if(!isThree)
                    targetImage.transform.localScale = new Vector3(1, 1, 1) * 1.8f; // 恢复缩放
                else
                    targetImage.transform.localScale = new Vector3(1, 1, 1) * 1.1f; // 恢复缩放

                if(targetImage.GetComponent<targetImage>().LastDragHandler != null){
                    targetImage.GetComponent<targetImage>().LastDragHandler.gameObject.SetActive(true);
                    transform.position = startPosition;
                    targetImage.GetComponent<targetImage>().LastDragHandler = this;
                    gameObject.SetActive(false);
                }
                else{
                    transform.position = startPosition;
                    targetImage.GetComponent<targetImage>().LastDragHandler = this;
                    gameObject.SetActive(false);
                }
                break; // 假设只能进入一个目标Image的区域，找到第一个后即停止
            }
        }
        transform.position = startPosition; // 将拖动的对象复位
    }

    void Start()
    {
        startPosition = transform.position; // 记录初始位置
    }
}
