using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag;
        var draggableItem = dropped.GetComponent<DraggableItem>();

        if (transform.childCount == 0)
            draggableItem.SetParent(transform);
        else
        {
            var itemInSlot = transform.GetChild(0).GetComponent<DraggableItem>();
            if (itemInSlot.GetItemType() == draggableItem.GetItemType())
            {
                MergeItems(itemInSlot, draggableItem);
            }
        }
    }

    private void MergeItems(DraggableItem itemInSlot, DraggableItem draggableItem)
    {
        Instantiate(draggableItem.GetNextLevelItem(), transform);
        Destroy(itemInSlot.gameObject); 
        Destroy(draggableItem.gameObject); 
    }
}