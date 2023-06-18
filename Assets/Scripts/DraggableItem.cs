using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action OnEndDragItem;
    public bool IsBlocked { get; private set; } = false;
    public ItemType Type => type;
    public DraggableItem NextLvlItem => nextLvlItem;
    public bool IsFinal => nextLvlItem == null || isFinal;

    [SerializeField]
    private Image image;

    [SerializeField]
    private DraggableItem nextLvlItem;

    [SerializeField]
    private ItemType type;

    private Transform _parentAfterDrag;
    private RectTransform rectTransform;
    private bool isFinal = false;

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsBlocked)
            return;

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
        OnEndDragItem?.Invoke();
    }

    public void SetNewSlot(Transform parent)
    {
        _parentAfterDrag = parent;
    }

    public void SetFinal()
    {
        isFinal = true;
    }

    public void Block()
    {
        IsBlocked = true;
        image.raycastTarget = false;
    }

    public void Unlock()
    {
        IsBlocked = false;
        image.raycastTarget = true;
    }

    public void Setup(DraggableItem item)
    {
        image.sprite = item.image.sprite;
        image.raycastTarget = true;
        nextLvlItem = item.NextLvlItem;
        type = item.Type;
    }
}