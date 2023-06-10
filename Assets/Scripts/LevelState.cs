using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LevelState : MonoBehaviour
{
    [SerializeField]
    private DraggableItem defaultItem;

    private List<InventorySlot> _items;
    private ObjectPool<DraggableItem> _pool;

    private void Awake()
    {
        _items = new List<InventorySlot>(FindObjectsOfType<InventorySlot>());
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
        newItem.Setup(itemInSlot.GetNextLevelItem());

        _pool.Release(itemInSlot);
        _pool.Release(draggableItem);
    }
}