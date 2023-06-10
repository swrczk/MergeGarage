using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDraggableItem
{
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

    public void SetParent(Transform parent)
    {
        _parentAfterDrag = parent;
    }

    public DraggableItem GetNextLevelItem() => nextLvlItem;

    public ItemType GetItemType() => type;
}