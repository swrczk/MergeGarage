using UnityEngine;

public class RewardSlot : MonoBehaviour
{
    [SerializeField] private DraggableItem _rewardItem;

    public bool IsEmpty => transform.childCount == 0;
    public DraggableItem RewardItem => _rewardItem;

    private void Start()
    {
        _rewardItem.Block();
    }

    public void SetAvailable()
    {
        _rewardItem.Unlock();
    }
}