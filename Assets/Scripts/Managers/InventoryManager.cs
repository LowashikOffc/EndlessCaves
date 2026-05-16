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

    // Добавляем ссылку на текущий активный предмет
    private GameObject _currentEquippedItem;

    public event Action OnInventoryChanged;
    public event Action<InventorySlot, int> InventorySetup;
    public event Action<GameObject> OnItemEquipped;
    public event Action<Vector3, Quaternion, GameObject> ChangeV3AndRot;
    public event Action<GameObject> OnItemUnequipped;

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

    private void Start()
    {
        Setup();
    }

    public void SetSelectedSlot(int index)
    {
        int previousIndex = SelectedSlotindex;
        SelectedSlotindex = Mathf.Clamp(index, 0, _slots.Count - 1);

        // Если слот изменился, обновляем экипированный предмет
        if (previousIndex != SelectedSlotindex)
        {
            EquipItemInSlot(SelectedSlotindex);
        }
    }

    public InventorySlot GetSelectedSlot()
    {
        return GetSlot(SelectedSlotindex);
    }

    // Получить текущий активный предмет в руках
    public GameObject GetCurrentEquippedItem()
    {
        return _currentEquippedItem;
    }

    // Проверить, является ли предмет активным
    public bool IsItemEquipped(GameObject item)
    {
        return _currentEquippedItem == item;
    }

    private void Setup()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            InventorySetup?.Invoke(_slots[i], i + 1);
        }

        // Экипируем предмет в первом слоте при старте
        if (_slots.Count > 0)
        {
            EquipItemInSlot(0);
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

        // Если удаляем предмет из активного слота - снимаем экипировку
        if (index == SelectedSlotindex && _currentEquippedItem != null)
        {
            UnequipCurrentItem();
        }

        ItemData data = _slots[index]._item;
        amount = _slots[index]._count;
        _slots.RemoveAt(index);
        UpdateTotalWeight();

        // Корректируем индекс если нужно
        if (SelectedSlotindex >= _slots.Count && _slots.Count > 0)
        {
            SelectedSlotindex = _slots.Count - 1;
        }

        OnInventoryChanged?.Invoke();
        Setup();

        // Экипируем новый предмет на текущем слоте
        if (_slots.Count > 0 && SelectedSlotindex < _slots.Count)
        {
            EquipItemInSlot(SelectedSlotindex);
        }
        else
        {
            UnequipCurrentItem();
        }

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
        Setup();
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

    // Экипировать предмет из указанного слота
    private void EquipItemInSlot(int slotIndex)
    {
        InventorySlot slot = GetSlot(slotIndex);
        if (slot == null || slot._item == null)
        {
            UnequipCurrentItem();
            return;
        }

        // Получаем компонент предмета в слоте
        // Предполагается, что у вас есть способ получить GameObject предмета из ItemData
        GameObject newItem = GetItemGameObject(slot._item);

        if (newItem == null) return;

        // Снимаем старый предмет
        if (_currentEquippedItem != null)
        {
            IEquippable oldEquippable = _currentEquippedItem.GetComponent<IEquippable>();
            if (oldEquippable != null)
            {
                oldEquippable.OnUnequip();
                Debug.Log($"Снят предмет: {_currentEquippedItem.name}");
            }
            OnItemUnequipped?.Invoke(_currentEquippedItem);
        }

        // Экипируем новый предмет
        _currentEquippedItem = newItem;
        IEquippable newEquippable = _currentEquippedItem.GetComponent<IEquippable>();
        if (newEquippable != null)
        {
            newEquippable.OnEquip();
            Debug.Log($"Экипирован предмет: {_currentEquippedItem.name}");
        }
        OnItemEquipped?.Invoke(_currentEquippedItem);
    }

    public void UpdateTransform(Vector3 pos, Quaternion rot)
    {
        ChangeV3AndRot?.Invoke(pos, rot, _currentEquippedItem);
    }

    // Снять текущий экипированный предмет
    private void UnequipCurrentItem()
    {
        if (_currentEquippedItem != null)
        {
            IEquippable equippable = _currentEquippedItem.GetComponent<IEquippable>();
            if (equippable != null)
            {
                equippable.OnUnequip();
                Debug.Log($"Снят предмет: {_currentEquippedItem.name}");
            }
            OnItemUnequipped?.Invoke(_currentEquippedItem);
            _currentEquippedItem = null;
        }
    }

    // Вспомогательный метод для получения GameObject предмета из ItemData
    // ВАМ НУЖНО РЕАЛИЗОВАТЬ ЭТОТ МЕТОД В СООТВЕТСТВИИ С ВАШЕЙ СИСТЕМОЙ
    private GameObject GetItemGameObject(ItemData itemData)
    {
        // Вариант 1: Если ItemData содержит префаб
        if (itemData._equipmentPrefab != null)
        {
            // Если предмет уже существует в сцене (в инвентаре), верните его
            // Иначе создайте новый экземпляр
            return itemData._equipmentPrefab;
        }

        // Вариант 2: Поиск по сцене
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            ItemData objItemData = obj.GetComponent<ItemData>();
            if (objItemData == itemData)
            {
                return obj;
            }
        }

        Debug.LogWarning($"Не найден GameObject для предмета: {itemData.name}");
        return null;
    }

    // Публичный метод для ручного переключения предмета
    public void SwitchToItem(GameObject newItem)
    {
        if (_currentEquippedItem == newItem) return;

        // Находим слот с этим предметом
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i]._item == GetItemDataFromGameObject(newItem))
            {
                SetSelectedSlot(i);
                return;
            }
        }
    }

    // Вспомогательный метод для получения ItemData из GameObject
    private ItemData GetItemDataFromGameObject(GameObject obj)
    {
        // Реализуйте в соответствии с вашей системой
        return obj.GetComponent<ItemData>();
    }
}