using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelState : MonoBehaviour
{
    // public event Action<int> OnLevelCompleted;
    // public event Action OnLevelFailed;

    [SerializeField] private Timer _timer;

    [SerializeField] private float _levelDuration;

    [SerializeField] private DraggableItem _defaultItem;

    [SerializeField] private GameObject _playgroundGroup;

    [SerializeField] private GameObject _questsGroup;

    [SerializeField] private LevelCompletedScreen _levelCompletedScreen;

    [SerializeField] private Canvas _levelFailedScreen;

    private readonly HashSet<ItemType> wantedTypes = new();
    private bool boardDirty = true;

    private int completedQuests;

    private GameSession gameSession;

    private List<InventorySlot> items;
    private ObjectPool<DraggableItem> pool;
    private List<QuestItem> quests;

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
        gameSession.ResetCurrency();

        _levelCompletedScreen.HideScreen();
        _levelFailedScreen.gameObject.SetActive(false);

        items = new List<InventorySlot>(_playgroundGroup.GetComponentsInChildren<InventorySlot>());
        quests = new List<QuestItem>(_questsGroup.GetComponentsInChildren<QuestItem>());

        quests.ForEach(q => q.OnQuestCompleted += OnQuestCompleted);

        pool = new ObjectPool<DraggableItem>(
            () => Instantiate(_defaultItem, transform),
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
        _timer.StartTimer(_levelDuration);
    }

    private void LateUpdate()
    {
        if (!boardDirty)
            return;

        boardDirty = false;
        HighlightWantedItems();
    }

    private void OnEnable()
    {
        DraggableItem.OnAnyItemMoved += MarkBoardDirty;
    }

    private void OnDisable()
    {
        DraggableItem.OnAnyItemMoved -= MarkBoardDirty;
    }

    private void MarkBoardDirty()
    {
        boardDirty = true;
    }

    private void HighlightWantedItems()
    {
        wantedTypes.Clear();
        foreach (var quest in quests)
        foreach (var type in quest.MissingItems)
            wantedTypes.Add(type);

        foreach (var slot in items)
        {
            var item = slot.Item;
            slot.SetHighlighted(item != null && !item.IsBlocked && wantedTypes.Contains(item.Type));
        }
    }

    public void OnTimeEnded()
    {
        Time.timeScale = 0;
        _levelFailedScreen.gameObject.SetActive(true);
        AudioManager.Play(Sfx.Lose);
    }

    public void RestartLevel()
    {
        LevelProgress.LoadLevel(LevelProgress.CurrentLevel);
    }

    // After the last level, back to the menu
    public void LoadNextLevel()
    {
        var next = LevelProgress.CurrentLevel + 1;
        if (next > LevelProgress.LevelCount)
            LevelProgress.LoadMenu();
        else
            LevelProgress.LoadLevel(next);
    }

    public void LoadMenu()
    {
        LevelProgress.LoadMenu();
    }

    public bool SpawnItem(DraggableItem item)
    {
        var slot = items.Find(x => x.IsEmpty);
        if (slot == null)
        {
            Debug.Log("No empty slots");
            return false;
        }


        var newItem = pool.Get();
        newItem.transform.SetParent(slot.transform, false);
        newItem.Setup(item);
        MarkBoardDirty();
        Debug.Log("Item spawned");
        return true;
    }

    public void Merge(DraggableItem itemInSlot, DraggableItem draggableItem, Transform parent)
    {
        var newItem = pool.Get();
        newItem.transform.SetParent(parent, false);
        newItem.Setup(itemInSlot.NextLvlItem);

        pool.Release(itemInSlot);
        pool.Release(draggableItem);
        MarkBoardDirty();
        AudioManager.Play(Sfx.Merge);
        AudioManager.Vibrate();
    }

    private void OnQuestCompleted()
    {
        Debug.Log("Quest completed");
        completedQuests++;
        if (completedQuests == quests.Count)
        {
            Debug.Log("All quests completed");
            Time.timeScale = 0;
            _timer.StopTimer();
            var timeBonus = Mathf.RoundToInt(_timer.CurrentTimeValue);
            _levelCompletedScreen.OnLevelCompleted(gameSession.Currency, timeBonus);
            gameSession.AddCurrency(timeBonus);
            LevelProgress.MarkCompleted(LevelProgress.CurrentLevel);
            AudioManager.Play(Sfx.Win);
        }
    }
}