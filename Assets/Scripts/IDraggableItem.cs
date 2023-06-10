using UnityEngine;

public interface IDraggableItem
{
    public ItemType GetItemType();
    public void SetParent(Transform parent);
    public DraggableItem GetNextLevelItem();
}