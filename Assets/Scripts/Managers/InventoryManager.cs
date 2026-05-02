using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> _slots = new List<InventorySlot>();
    [SerializeField] private float _maxWeight = 50f;
    [SerializeField] float _currentWeight;

    public bool TryAddItem(ItemData newItem, int amount)
    {
        float weightToAdd = newItem.weight * amount;
        if (_currentWeight + weightToAdd > _maxWeight)
        {
            Debug.Log("Слишком большой вес");
            return false;
        }
        int remainingAount = amount;
        foreach (InventorySlot slot in _slots)
        {
            if (slot._item == newItem && !slot.IsFull())
            {
                int canAdd = newItem._maxStackSize - slot._count;
                int toAdd = Mathf.Min(canAdd, remainingAount);
                slot._count += toAdd;
                remainingAount -= toAdd;
            }
            if (remainingAount <= 0) break;
        }
        while (remainingAount > 0)
        {
            int toAdd = Mathf.Min(newItem._maxStackSize, remainingAount);
            _slots.Add(new InventorySlot(newItem, toAdd));
            remainingAount -= toAdd;
        }
        UpdateTotalWeight();
        return true;
    }
    private void UpdateTotalWeight()
    {
        _currentWeight = 0;
        foreach (InventorySlot slot in _slots)
        {
            _currentWeight += slot.GetTotalWeight();
        }
    }
}
