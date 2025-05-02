using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// This class is used to handle button event such as hover(pointer enter), click(pointer down), release from click (pointer up),
/// and release from hover (pointer exit)
/// </summary>
public class PointerHandlerBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnPointerEnterEvent, OnPointerUpEvent, OnPointerExitEvent, OnPointerDownEvent;

    public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterEvent?.Invoke();
    public void OnPointerExit(PointerEventData eventData) => OnPointerExitEvent?.Invoke();
    public void OnPointerDown(PointerEventData eventData) => OnPointerDownEvent?.Invoke();
    public void OnPointerUp(PointerEventData eventData) => OnPointerUpEvent?.Invoke();
}