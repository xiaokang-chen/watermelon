using UnityEngine;
using UnityEngine.EventSystems;

public class VideoSliderManager : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public delegate void VideoSliderDragDelegate(PointerEventData eventData);

    public event VideoSliderDragDelegate OnDragListener, OnEndDragListener;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update(){}

    public void OnDrag(PointerEventData eventData)
    {
        OnDragListener?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragListener?.Invoke(eventData);
    }
}
