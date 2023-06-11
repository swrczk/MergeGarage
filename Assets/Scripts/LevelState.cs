using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class LevelState : MonoBehaviour
{
    [SerializeField]
    private DraggableItem defaultItem;

    [SerializeField]
    private GameObject playgroundGroup;

    [SerializeField]
    private GameObject questsGroup;

    [SerializeField]
    private GameObject congratulationsScreen;

    private List<InventorySlot> _items;
    private List<QuestItem> _quests;
    private ObjectPool<DraggableItem> _pool;
    
    private int completedQuests = 0;

    private void Awake()
    {
        congratulationsScreen.SetActive(false);
        
        _items = new List<InventorySlot>(playgroundGroup.GetComponentsInChildren<InventorySlot>());
        _quests = new List<QuestItem>(questsGroup.GetComponentsInChildren<QuestItem>());
        
        _quests.ForEach(q=>q.OnQuestCompleted += OnQuestCompleted);
        
        _pool = new ObjectPool<DraggableItem>(
            () => Instantiate(defaultItem, transform),
            item => item.gameObject.SetActive(true),
            item =>
            {
                item.gameObject.SetActive(false);
                item.transform.SetParent(transform);
            },
            item => Destroy(item.gameObject),
            false, 10, 20
        );
    }

    public bool SpawnItem(DraggableItem item)
    {
        var slot = _items.Find(x => x.IsEmpty);
        if (slot == null)
        {
            Debug.Log("No empty slots");
            return false;
        }


        var newItem = _pool.Get();
        newItem.transform.SetParent(slot.transform);
        newItem.Setup(item);
        return true;
    }

    public void Merge(DraggableItem itemInSlot, DraggableItem draggableItem, Transform parent)
    {
        var newItem = _pool.Get();
        newItem.transform.SetParent(parent);
        newItem.Setup(itemInSlot.NextLvlItem);

        _pool.Release(itemInSlot);
        _pool.Release(draggableItem);
    }

    private void OnQuestCompleted()
    {
        Debug.Log("Quest completed");
        completedQuests++;
        if (completedQuests == _quests.Count)
        {
            Debug.Log("All quests completed");
            congratulationsScreen.SetActive(true);
        }
    }
}