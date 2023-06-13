using UnityEngine;

public class RewardSlot : MonoBehaviour
{
    public bool IsEmpty => transform.childCount == 0;
    public DraggableItem RewardItem => rewardItem;

    [SerializeField]
    private DraggableItem rewardItem; 

    private void Start()
    {
        rewardItem.Block();
    }

    public void SetAvailable()
    { 
        rewardItem.Unlock();
    }
}