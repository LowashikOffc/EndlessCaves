using UnityEngine;

// ScriptableObject для описания базовых данных предмета
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private GameObject _model;
    [SerializeField] private bool _isStackable;
    public virtual void Use()
    {
        Debug.Log($"Использован предмет: {_itemName}");
    }

    public GameObject ModelGet()
    {
        return _model;
    }
}