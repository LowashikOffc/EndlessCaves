using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData _item;
    public int _count;

    public InventorySlot(ItemData data, int amount)
    {
        _item = data;
        _count = amount;
    }

    public float GetTotalWeight() => _item != null ? _item.weight * _count : 0f;
    public bool IsFull() => _item != null && _count >= _item._maxStackSize;
}
