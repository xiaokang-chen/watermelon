using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public Action<PointerEventData> onDown;
    public Action<PointerEventData> onUp;
    public Action<PointerEventData> onDrag;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        onDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onUp?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData);
    }
}
