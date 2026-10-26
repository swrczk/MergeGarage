using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestInventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private DraggableItem _requiredItem;

    [SerializeField] private Image _previewImage;

    [SerializeField] private InventorySlot _inventorySlot;

    [SerializeField] private float _scale = 0.5f;

    [SerializeField] private Color _slotCompletedColor;

    [SerializeField] private Sprite _slotCompletedSprite;

    [SerializeField] private GameObject _completedMark;

    public bool IsEmpty { get; private set; } = true;

    public ItemType RequiredType => _requiredItem.Type;

    private void Start()
    {
        _previewImage.transform.localScale = _scale * Vector3.one;
        _previewImage.sprite = _requiredItem.GetComponent<Image>().sprite;
    }

    private void OnDestroy()
    {
        OnItemAdded = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dropped = eventData.pointerDrag;
        var draggableItem = dropped.GetComponent<DraggableItem>();

        if (AnyIsBlocked(draggableItem, _requiredItem))
            return;

        if (draggableItem.Type == _requiredItem.Type && IsEmpty)
        {
            IsEmpty = false;
            draggableItem.transform.localScale = _scale * Vector3.one;
            draggableItem.SetFinal();
            _inventorySlot.OnDrop(eventData);
            ShowCompleted();
            OnItemAdded?.Invoke();
        }
        else if (IsEmpty)
        {
            Debug.Log("Wrong item");
            ShakeFeedback.Play(_previewImage);
            AudioManager.Play(Sfx.Error);
        }
    }

    public event Action OnItemAdded;

    public void ShowCompleted()
    {
        var slotImage = _inventorySlot.GetComponent<Image>();
        if (_slotCompletedSprite != null)
            slotImage.sprite = _slotCompletedSprite;
        slotImage.color = _slotCompletedColor;
        _previewImage.enabled = false;
        if (_completedMark != null)
            _completedMark.SetActive(true);
    }

    private bool AnyIsBlocked(DraggableItem draggableItem, DraggableItem requiredItem)
    {
        return draggableItem.IsBlocked || requiredItem.IsBlocked;
    }
}