using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SlotSelect(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SlotSelect(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SlotSelect(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SlotSelect(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SlotSelect(4);
        }
    }

    private void SlotSelect(int slotNumber)
    {
        _currentSlot = slotNumber;
        UISlotChange?.Invoke(slotNumber);
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