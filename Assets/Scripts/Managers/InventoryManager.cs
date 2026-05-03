using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private List<InventorySlot> _slots = new List<InventorySlot>();
    [SerializeField] private float _maxWeight = 50f;
    [SerializeField] float _currentWeight;
    public int SelectedSlotindex { get; private set; } = 0;

    public event Action OnInventoryChanged;
    public event Action<InventorySlot, int> InventorySetup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedSlot(int index)
    {
        SelectedSlotindex = Mathf.Clamp(index, 0, _slots.Count - 1);
    }

    public InventorySlot GetSelectedSlot()
    {
        return GetSlot(SelectedSlotindex);
    }

    private void Setup()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            InventorySetup?.Invoke(_slots[i], i+1);
        }
    }

    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < _slots.Count)
        {
            return _slots[index];
        }
        return null;
    }

    public ItemData RemoveItem(int index, out int amount)
    {
        amount = 0;
        if (index < 0 || index >= _slots.Count) return null;
        ItemData data = _slots[index]._item;
        amount = _slots[index]._count;
        _slots.RemoveAt(index);
        UpdateTotalWeight();

        OnInventoryChanged?.Invoke();
        Setup();

        return data;
    }

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
        OnInventoryChanged?.Invoke();
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
