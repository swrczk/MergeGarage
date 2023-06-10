using UnityEngine;
using UnityEngine.EventSystems;

public class Spawner: MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private DraggableItem item;
    
    private LevelState _levelState;

    private void Awake()
    {
        _levelState = FindObjectOfType<LevelState>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _levelState.SpawnItem(item);
    }
}