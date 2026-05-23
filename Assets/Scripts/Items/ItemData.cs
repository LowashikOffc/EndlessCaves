using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string _itemName;
    public Texture _icon;
    public int _maxStackSize = 99;
    public string itemID;
    public float weight = 1f;
    public GameObject _equipmentPrefab;

    public virtual void Use()
    {
        Debug.Log($"Используется предмет: {_itemName}");
    }
    private void OnValidate()
    {
        // Автоматически генерируем ID если его нет
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = Guid.NewGuid().ToString();
        }
    }
}


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class HealItemData : ItemData
{
    public int _healAmount = 20;
}