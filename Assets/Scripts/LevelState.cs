using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelState : MonoBehaviour
{
    // public event Action<int> OnLevelCompleted;
    // public event Action OnLevelFailed;

    [SerializeField]
    private Timer timer;

    [SerializeField]
    private float levelDuration;

    [SerializeField]
    private DraggableItem defaultItem;

    [SerializeField]
    private GameObject playgroundGroup;

    [SerializeField]
    private GameObject questsGroup;

    [SerializeField]
    private LevelCompletedScreen levelCompletedScreen;

    [SerializeField]
    private Canvas levelFailedScreen;

    private List<InventorySlot> _items;
    private List<QuestItem> _quests;
    private ObjectPool<DraggableItem> _pool;

    private int completedQuests = 0;

    private GameSession _gameSession;

    private void Awake()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _gameSession.ResetCurrency();

        levelCompletedScreen.HideScreen();
        levelFailedScreen.gameObject.SetActive(false);

        _items = new List<InventorySlot>(playgroundGroup.GetComponentsInChildren<InventorySlot>());
        _quests = new List<QuestItem>(questsGroup.GetComponentsInChildren<QuestItem>());

        _quests.ForEach(q => q.OnQuestCompleted += OnQuestCompleted);

        _pool = new ObjectPool<DraggableItem>(
            () => Instantiate(defaultItem, transform),
            item => item.gameObject.SetActive(true),
            item =>
            {
                item.gameObject.SetActive(false);
                item.transform.SetParent(transform);
            },
            item => Destroy(item.gameObject),
            false, 50, 60
        );
        Time.timeScale = 1;
    }

    private void Start()
    {
        timer.StartTimer(levelDuration);
    }

    public void OnTimeEnded()
    {
        Time.timeScale = 0;
        levelFailedScreen.gameObject.SetActive(true);
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
        newItem.transform.SetParent(slot.transform, false);
        newItem.Setup(item);
        Debug.Log("Item spawned");
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
            levelCompletedScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
            timer.StopTimer();
            var timeBonus = Mathf.RoundToInt(timer.CurrentTimeValue);
            levelCompletedScreen.OnLevelCompleted(_gameSession.Currency, timeBonus);
        }
    }
}