using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Sprite _highlightSprite;

    private bool highlighted;
    private Image image;

    private LevelState levelState;
    private Sprite normalSprite;

    public bool IsEmpty => transform.childCount == 0;
    public DraggableItem Item => IsEmpty ? null : transform.GetChild(0).GetComponent<DraggableItem>();

    private void Awake()
    {
        levelState = FindObjectOfType<LevelState>();
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

        if (AnyIsFinal(draggableItem, itemInSlot))
            return;

        if (itemInSlot.Type == draggableItem.Type) levelState.Merge(itemInSlot, draggableItem, transform);
    }

    // Marks the slot when its item is wanted by an open quest
    public void SetHighlighted(bool highlighted)
    {
        if (highlighted == this.highlighted || _highlightSprite == null)
            return;

        if (image == null)
        {
            image = GetComponent<Image>();
            normalSprite = image.sprite;
        }

        this.highlighted = highlighted;
        image.sprite = highlighted ? _highlightSprite : normalSprite;
    }

    private bool AnyIsBlocked(DraggableItem draggableItem, DraggableItem requiredItem)
    {
        return draggableItem.IsBlocked || requiredItem.IsBlocked;
    }

    private bool AnyIsFinal(DraggableItem draggableItem, DraggableItem requiredItem)
    {
        return draggableItem.IsFinal || requiredItem.IsFinal;
    }
}