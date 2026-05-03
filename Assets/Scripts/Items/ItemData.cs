using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string _itemName;
    public Texture _icon;
    public int _maxStackSize = 99;
    public float weight = 1f;
    public GameObject _equipmentPrefab;

    public virtual void Use()
    {
        Debug.Log($"Используется предмет: {_itemName}");
    }
}


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class HealItemData : ItemData
{
    public int _healAmount = 20;
}