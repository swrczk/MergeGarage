using UnityEngine;
using UnityEngine.EventSystems;

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
        {
            draggableItem.SetNewSlot(transform);
            return;
        }

        var itemInSlot = transform.GetChild(0).GetComponent<DraggableItem>();

        if (AnyIsBlocked(draggableItem, itemInSlot))
            return;
        
        if (itemInSlot.Type == draggableItem.Type)
        {
            _levelState.Merge(itemInSlot, draggableItem, transform);
        }
    }

    private bool AnyIsBlocked(DraggableItem draggableItem, DraggableItem requiredItem)
    {
        return draggableItem.IsBlocked || requiredItem.IsBlocked;
    }
}