using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestInventorySlot : MonoBehaviour, IDropHandler
{
    public event Action OnItemAdded;
    
    public bool IsEmpty { get; private set; } = true;

    [SerializeField]
    private DraggableItem requiredItem;

    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private InventorySlot inventorySlot;

    private void Start()
    {
        previewImage.transform.localScale = 0.8f * Vector3.one;
        previewImage.sprite = requiredItem.GetComponent<Image>().sprite;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag;
        var draggableItem = dropped.GetComponent<DraggableItem>();
        
        if(AnyIsBlocked(draggableItem, requiredItem))
            return;
        
        if (draggableItem.Type == requiredItem.Type && IsEmpty)
        {
            IsEmpty = false;
            draggableItem.transform.localScale = 0.8f * Vector3.one;
            inventorySlot.OnDrop(eventData);
            OnItemAdded?.Invoke();
        }
        else
        {
            Debug.Log("Wrong item");
        }
    }

    private bool AnyIsBlocked(DraggableItem draggableItem, DraggableItem requiredItem)
    {
        return draggableItem.IsBlocked || requiredItem.IsBlocked;
    }

    private void OnDestroy()
    {
        OnItemAdded = null;
    }
}
