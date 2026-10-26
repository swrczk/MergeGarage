using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;

    [SerializeField] private DraggableItem _nextLvlItem;

    [SerializeField] private ItemType _type;

    private bool isFinal;

    private Transform parentAfterDrag;
    private RectTransform rectTransform;
    public bool IsBlocked { get; private set; }
    public ItemType Type => _type;
    public DraggableItem NextLvlItem => _nextLvlItem;
    public bool IsFinal => _nextLvlItem == null || isFinal;

    private void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsBlocked)
            return;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
        OnAnyItemMoved?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsBlocked)
            return;

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsBlocked)
            return;

        transform.SetParent(parentAfterDrag);
        _image.raycastTarget = true;
        OnEndDragItem?.Invoke();
        OnAnyItemMoved?.Invoke();
    }

    public event Action OnEndDragItem;

    // Fired whenever any item leaves or returns to a slot by dragging
    public static event Action OnAnyItemMoved;

    public void SetNewSlot(Transform parent)
    {
        parentAfterDrag = parent;
    }

    public void SetFinal()
    {
        isFinal = true;
    }

    public void Block()
    {
        IsBlocked = true;
        _image.raycastTarget = false;
    }

    public void Unlock()
    {
        IsBlocked = false;
        _image.raycastTarget = true;
    }

    public void Setup(DraggableItem item)
    {
        // Pooled items keep state from their previous use, so reset everything here
        _image.sprite = item._image.sprite;
        _nextLvlItem = item.NextLvlItem;
        _type = item.Type;
        isFinal = false;
        transform.localScale = Vector3.one;
        Unlock();
    }
}