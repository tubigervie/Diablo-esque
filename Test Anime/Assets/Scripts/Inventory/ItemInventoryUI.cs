using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInventoryUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 _startPosition;
    Transform _originalParent;

    Canvas _parentCanvas;
    InventorySlotUI _parentSlot;

    private void Awake()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
        _parentSlot = GetComponentInParent<InventorySlotUI>();
    }

    public InventorySlotUI parentSlot { get { return _parentSlot; } }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = transform.position;
        _originalParent = transform.parent;
        transform.SetParent(_parentCanvas.transform, true);
        // Else won't get the drop event.
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _startPosition;
        transform.SetParent(_originalParent, true);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Debug.Log("Before drop");
        // Not over UI we should drop.
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _parentSlot.DiscardItem();
            Debug.Log("After drop");
        }

    }
}
   