using System;
using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

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

    private int _currentSlot;
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private int _maxItemCount;

    public event Action<int> UISlotChange;
    public event Action<GameObject, int> UIModelAdd;
    public event Action<int> UIModelRemove;

    // Делегат для уведомления об изменениях в инвентаре
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    private void Start()
    {
        InputReceiver.Instance.SlotSelect += SlotSelect;
    }
    private void SlotSelect(int slotNumber)
    {
        _currentSlot = slotNumber - 1;
        UISlotChange?.Invoke(_currentSlot);
    }
    // Метод для добавления предмета
    public bool Add(Item item)
    {
        if (_items.Count >= _maxItemCount)
        {
            Debug.Log("Нет места в инвентаре!");
            return false;
        }

        _items.Add(item);
        onItemChangedCallback?.Invoke(); // Уведомляем об изменениях
        GameObject modelToCreate = item.ModelGet();
        UIModelAdd?.Invoke(modelToCreate, _items.Count-1);
        Debug.Log(modelToCreate);
        return true;
    }

    // Метод для удаления предмета
    public void Remove()
    {
        int slotToRemove =  _currentSlot;
        Item itemToRemove = _items[slotToRemove];
        GameObject modelToDelete = itemToRemove.ModelGet();
        Debug.Log(modelToDelete);
        UIModelRemove?.Invoke(slotToRemove);
        _items.Remove(itemToRemove);
        onItemChangedCallback?.Invoke(); // Уведомляем об изменениях
        
    }
}