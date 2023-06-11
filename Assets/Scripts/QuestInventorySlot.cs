using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestInventorySlot : MonoBehaviour, IDropHandler
{
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
        if (draggableItem.Type == requiredItem.Type)
        {
            draggableItem.transform.localScale = 0.8f * Vector3.one;
            inventorySlot.OnDrop(eventData);
        }
        else
        {
            Debug.Log("Wrong item");
        }
    }
}
