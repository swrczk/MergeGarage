using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    [SerializeField] private List<QuestInventorySlot> _questInventorySlots;

    [SerializeField] private RewardSlot _rewardSlot;

    [SerializeField] private int _rewardCurrency;

    [Header("UI")] [SerializeField] private CurrencyInfoElement _currencyInfoElement;

    [SerializeField] private Image _background;

    [SerializeField] private Color _finishedQuestColor;

    [SerializeField] private Color _rewardReadyToCollectColor;

    private int filledSlots;

    private GameSession gameSession;

    public IEnumerable<ItemType> MissingItems
    {
        get
        {
            foreach (var slot in _questInventorySlots)
                if (slot.IsEmpty)
                    yield return slot.RequiredType;
        }
    }

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();

        _currencyInfoElement.ChangeCurrencyText(_rewardCurrency);
        _questInventorySlots.ForEach(x => x.OnItemAdded += OnItemAdded);
        if (_rewardSlot != null) _rewardSlot.RewardItem.OnEndDragItem += OnRewardItemDragged;
    }

    private void OnDestroy()
    {
        OnQuestCompleted = null;
    }

    public event Action OnQuestCompleted;

    private void OnRewardItemDragged()
    {
        if (_rewardSlot.IsEmpty) CompleteQuest();
    }

    private void OnItemAdded()
    {
        filledSlots++;

        if (filledSlots != _questInventorySlots.Count) return;

        if (_rewardSlot != null)
        {
            transform.SetAsFirstSibling();
            _background.color = _rewardReadyToCollectColor;
            _rewardSlot.SetAvailable();
        }
        else
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        Debug.Log("Quest completed");
        _background.color = _finishedQuestColor;
        transform.SetAsLastSibling();
        gameSession.AddCurrency(_rewardCurrency);
        AudioManager.Play(Sfx.QuestDone);
        OnQuestCompleted?.Invoke();
    }
}