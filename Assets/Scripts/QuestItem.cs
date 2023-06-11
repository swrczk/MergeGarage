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
    private Image background;

    [SerializeField]
    private Color finishedQuestColor;

    private int _filledSlots = 0;

    private void Awake()
    {
        questInventorySlots.ForEach(x => x.OnItemAdded += OnItemAdded);
    }

    private void OnItemAdded()
    {
        _filledSlots++;
        if (_filledSlots == questInventorySlots.Count)
        {
            Debug.Log("Quest completed");
            background.color = finishedQuestColor;
            transform.SetAsLastSibling();
            OnQuestCompleted?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnQuestCompleted = null;
    }
}