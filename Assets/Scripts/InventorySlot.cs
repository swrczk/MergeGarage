using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private LevelState _levelState;
    public bool IsEmpty => transform.childCount == 0;

    private void Awake()
    {
        _levelState = FindObjectOfType<LevelState>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag;
        var draggableItem = dropped.GetComponent<DraggableItem>();

        if (IsEmpty)
            draggableItem.SetNewSlot(transform);
        else
        {
            var itemInSlot = transform.GetChild(0).GetComponent<DraggableItem>();
            if (itemInSlot.GetItemType() == draggableItem.GetItemType())
            {
                _levelState.Merge(itemInSlot, draggableItem, transform);
            }
        }
    }
}