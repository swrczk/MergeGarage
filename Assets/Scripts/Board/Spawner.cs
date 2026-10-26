using UnityEngine;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private DraggableItem _item;

    private LevelState levelState;

    private void Awake()
    {
        levelState = FindObjectOfType<LevelState>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Spawner clicked");
        if (levelState.SpawnItem(_item))
        {
            AudioManager.Play(Sfx.Spawn);
        }
        else
        {
            ShakeFeedback.Play(this);
            AudioManager.Play(Sfx.Error);
        }
    }
}