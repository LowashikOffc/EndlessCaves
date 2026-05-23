using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance { get; private set; }
    [SerializeField] private Transform _handTransform;
    [SerializeField] private float _dropDistance;
    [SerializeField] private float _dropForce;

    private IEquippable _currentActiveItem;
    private int _currentSlotIndex = -1;

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
        InputReceiver.Instance.SlotSelect += HandleSlotSelect;
        InputReceiver.Instance.Drop += DropCurrentItem;

        // Подписываемся на события из InventoryManager
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemEquipped += HandleItemEquipped;
            InventoryManager.Instance.OnItemUnequipped += HandleItemUnequipped;
        }
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.SlotSelect -= HandleSlotSelect;
            InputReceiver.Instance.Drop -= DropCurrentItem;
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnItemEquipped -= HandleItemEquipped;
            InventoryManager.Instance.OnItemUnequipped -= HandleItemUnequipped;
        }
    }

    // Обработчик экипировки из InventoryManager
    private void HandleItemEquipped(GameObject itemPrefab)
    {
        // 1. Сначала уничтожаем то, что УЖЕ было в руке
        CleanUpCurrentActiveItem();

        // 2. Если нам передали валидный префаб
        if (itemPrefab != null && _handTransform != null)
        {
            // Создаем РЕАЛЬНЫЙ объект в руке игрока
            GameObject spawnedObj = Instantiate(itemPrefab, _handTransform);
            spawnedObj.transform.localPosition = Vector3.zero;
            spawnedObj.transform.localRotation = Quaternion.identity;

            if (spawnedObj.TryGetComponent(out Rigidbody rb))
                rb.isKinematic = true;

            // Получаем интерфейс и запускаем OnEquip у реального объекта на сцене
            _currentActiveItem = spawnedObj.GetComponent<IEquippable>();
            if (_currentActiveItem != null)
            {
                _currentActiveItem.OnEquip();
            }

            //Debug.Log($"PlayerEquipmentManager: Спавн объекта {spawnedObj.name} в руке");
        }
    }

    // Обработчик снятия предмета
    private void HandleItemUnequipped(GameObject itemPrefab)
    {
        CleanUpCurrentActiveItem();
    }

    // Отдельный метод очистки руки, чтобы не дублировать код
    private void CleanUpCurrentActiveItem()
    {
        if (_currentActiveItem != null)
        {
            var itemBehavior = _currentActiveItem as MonoBehaviour;
            if (itemBehavior != null)
            {
                // Вызываем OnUnequip перед тем как уничтожить объект!
                _currentActiveItem.OnUnequip();
                Destroy(itemBehavior.gameObject);
            }
            _currentActiveItem = null;
        }
    }

    public void DropCurrentItem()
    {
        // Запрашиваем индекс текущего выбранного слота напрямую у InventoryManager
        int activeIndex = InventoryManager.Instance.SelectedSlotindex;

        // Проверяем, есть ли там вообще предмет
        var slot = InventoryManager.Instance.GetSlot(activeIndex);
        if (slot == null || slot._item == null) return;

        ItemData itemToDrop = InventoryManager.Instance.RemoveItem(activeIndex, out int amount);

        if (itemToDrop != null)
        {
            // Спавним выкинутый предмет в мир
            for (int i = 0; i < amount; i++)
            {
                SpawnDroppedItem(itemToDrop, 1);
            }
        }
    }

    private void SpawnDroppedItem(ItemData data, int amount)
    {
        Vector3 spawnPos = _handTransform.position + Camera.main.transform.forward * _dropDistance;
        GameObject worldObj = Instantiate(data._equipmentPrefab, spawnPos, Quaternion.identity);

        if (worldObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Camera.main.transform.forward * _dropForce, ForceMode.Impulse);
        }

        if (worldObj.TryGetComponent(out ItemObject itemObj))
        {
            itemObj._data = data;
            itemObj._amount = amount;
        }

        if (worldObj.TryGetComponent(out Collider col))
            col.enabled = true;
    }

    public void RegisterHand(Transform hand)
    {
        _handTransform = hand;
    }

    // ИСПРАВЛЕННЫЙ МЕТОД: Только выбираем слот, без прямой экипировки
    private void HandleSlotSelect(int slotNumber)
    {
        _currentSlotIndex = slotNumber - 1;

        if (InventoryManager.Instance == null) return;

        // Просто вызываем выбор слота в InventoryManager
        // Вся логика экипировки теперь в InventoryManager
        InventoryManager.Instance.SetSelectedSlot(_currentSlotIndex);
    }
}