using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ItemPickUp _itemPickUp;
    [SerializeField] private GameObject _inventoryBar;
    [SerializeField] private GameObject _selectFrame;
    [SerializeField] private TMP_Text _mainText;
    [SerializeField] private TMP_Text _subText;
    [SerializeField] private List<GameObject> _slots;
    [SerializeField] private List<GameObject> _items;
    [SerializeField] private GameObject _selectedSlot;

    private void Start()
    {
        Inventory.Instance.UISlotChange += ChangeSelectedSlot;
        Inventory.Instance.UIModelAdd += AddModel;
        Inventory.Instance.UIModelRemove += RemoveModel;
        _itemPickUp.UIChangeText += ChangeText;
    }

    private void ChangeText(string text, string subText)
    {
        _mainText.text = text;
        _subText.text = subText;
    }

    private void ChangeSelectedSlot(int slotNumber)
    {
        Debug.Log(slotNumber);
        _selectedSlot = _slots[slotNumber];
    }

    private void LateUpdate()
    {   
        _selectFrame.transform.position = Vector2.Lerp(_selectFrame.transform.position, _selectedSlot.transform.position, Time.deltaTime * 30);
    }

    private void AddModel(GameObject model, int slot)
    {
        Debug.Log("Add "+model+"in slot "+slot);
        GameObject newModel = Instantiate(model);
        Debug.Log(model.name);
        _items[slot] = newModel;
    }
    private void RemoveModel(int slot)
    {
        Debug.Log("Remove from slot " + slot);
        _items[slot] = null;
    }
}
