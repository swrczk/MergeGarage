using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemType Type => type;
    public DraggableItem NextLvlItem => nextLvlItem;
    [SerializeField]
    private Image image;

    [SerializeField]
    private DraggableItem nextLvlItem;

    [SerializeField]
    private ItemType type;

    private Transform _parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_parentAfterDrag);
        image.raycastTarget = true;
    }

    public void SetNewSlot(Transform parent)
    {
        _parentAfterDrag = parent;
    }

    public void Setup(DraggableItem item)
    {
        image.sprite = item.image.sprite;
        image.raycastTarget = true;
        nextLvlItem = item.NextLvlItem;
        type = item.Type;
    }
}