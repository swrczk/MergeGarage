using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    public event Action OnQuestCompleted;

    [SerializeField]
    private List<QuestInventorySlot> questInventorySlots;

    [SerializeField]
    private RewardSlot rewardSlot;

    [SerializeField]
    private Image background;

    [SerializeField]
    private Color finishedQuestColor;

    [SerializeField]
    private Color rewardReadyToCollectColor;

    private int _filledSlots = 0;

    private void Awake()
    {
        questInventorySlots.ForEach(x => x.OnItemAdded += OnItemAdded);
        if (rewardSlot != null)
        {
            rewardSlot.RewardItem.OnEndDragItem += OnRewardItemDragged;
        }
    }

    private void OnRewardItemDragged()
    {
        if (rewardSlot.IsEmpty)
        {
            CompleteQuest();
        }
    }

    private void OnItemAdded()
    {
        _filledSlots++;
        
        if (_filledSlots != questInventorySlots.Count) return;
        
        if(rewardSlot != null)
        {
            transform.SetAsFirstSibling();
            background.color = rewardReadyToCollectColor;
            rewardSlot.SetAvailable();
        }
        else
            CompleteQuest();
    }

    private void CompleteQuest()
    {
        Debug.Log("Quest completed");
        background.color = finishedQuestColor;
        transform.SetAsLastSibling();
        OnQuestCompleted?.Invoke();
    }

    private void OnDestroy()
    {
        OnQuestCompleted = null;
    }
}