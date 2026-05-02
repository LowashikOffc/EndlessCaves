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
    }

    private void OnDestroy()
    {

        InputReceiver.Instance.SlotSelect -= HandleSlotSelect;
        InputReceiver.Instance.Drop -= DropCurrentItem;
    }

    public void DropCurrentItem()
    {
        // Если ничего не выбрано или индекс некорректен — отмена
        if (_currentSlotIndex == -1) return;

        // 1. Пытаемся забрать данные из инвентаря
        ItemData itemToDrop = InventoryManager.Instance.RemoveItem(_currentSlotIndex, out int amount);

        if (itemToDrop != null)
        {
            // 2. Удаляем визуал из руки
            if (_currentActiveItem != null)
            {
                var itemBehavior = _currentActiveItem as MonoBehaviour;
                if (itemBehavior != null) Destroy(itemBehavior.gameObject);
                _currentActiveItem = null;
            }

            for (int i = 0; i < amount; i++)
            {
                SpawnDroppedItem(itemToDrop, 1);
            }

            // Сбрасываем индекс, так как этого предмета больше нет
            _currentSlotIndex = -1;
        }
    }

    private void SpawnDroppedItem(ItemData data, int amount)
    {
        // Позиция: перед игроком
        Vector3 spawnPos = _handTransform.position + Camera.main.transform.forward * _dropDistance;
        GameObject worldObj = Instantiate(data._equipmentPrefab, spawnPos, Quaternion.identity);

        // Настройка физики (включаем её обратно)
        if (worldObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Camera.main.transform.forward * _dropForce, ForceMode.Impulse);
        }

        // Настройка скрипта подбора
        if (worldObj.TryGetComponent(out ItemObject itemObj))
        {
            itemObj._data = data;
            itemObj._amount = amount;
        }

        // Включаем коллайдер, чтобы предмет не провалился
        if (worldObj.TryGetComponent(out Collider col)) col.enabled = true;
    }

    public void RegisterHand(Transform hand)
    {
        _handTransform = hand;
    }

    private void HandleSlotSelect(int slotNumber)
    {
        // Сохраняем индекс для будущего использования (Drop)
        _currentSlotIndex = slotNumber - 1;

        if (InventoryManager.Instance == null) return;

        var slot = InventoryManager.Instance.GetSlot(_currentSlotIndex);

        // Очистка руки
        if (_currentActiveItem != null)
        {
            _currentActiveItem.OnUnequip();
            var itemBehavior = _currentActiveItem as MonoBehaviour;
            if (itemBehavior != null) Destroy(itemBehavior.gameObject);
            _currentActiveItem = null;
        }

        // Если слот пуст — выходим
        if (slot == null || slot._item == null) return;

        // Спавн предмета
        if (_handTransform != null && slot._item._equipmentPrefab != null)
        {
            GameObject obj = Instantiate(slot._item._equipmentPrefab, _handTransform);

            // Сразу обнуляем позицию и физику
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            if (obj.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;

            _currentActiveItem = obj.GetComponent<IEquippable>();
            _currentActiveItem?.OnEquip();
        }
    }

    private void EquipItem(int index)
    {
        if (_handTransform == null) return;
        if (_currentSlotIndex != 0)
        {
            _currentActiveItem.OnUnequip();
            Destroy((_currentActiveItem as MonoBehaviour).gameObject);
            _currentActiveItem = null;
        }
        var slot = InventoryManager.Instance.GetSlot(index);

        if (slot != null && slot._item != null && slot._item._equipmentPrefab != null)
        {
            GameObject instance = Instantiate(slot._item._equipmentPrefab, _handTransform);
            _currentActiveItem = Instance.GetComponent<IEquippable>();
            if (_currentActiveItem != null)
            {
                _currentActiveItem.OnEquip();
            }
        }
    }
}
