using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{

    [SerializeField] private RawImage _iconDisplay;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _slotNumber;
    [SerializeField] private GameObject _countObject;
    [SerializeField] private GameObject _selectionVisual;

    public void UpdateVisual(InventorySlot slot, int slotIndex)
    {
        _slotNumber.text = slotIndex.ToString();
        //Debug.Log($"Обновление ячейки номер: {slotIndex}");
        if (slot != null && slot._item != null && slot._item._icon != null)
        {
            //Debug.Log(slot._item._icon);
            _iconDisplay.texture = slot._item._icon;
            _iconDisplay.enabled = true;
            _countText.text = slot._count.ToString();
            _countObject.SetActive(slot._count > 1);
        }
        else
        {
            _iconDisplay.texture = null;
            _iconDisplay.enabled = false;
            _countObject.SetActive(false);
        }
    }

    public void UpdateSlot(InventorySlot slot)
    {
        if (slot == null || slot._item == null || slot._count <= 0)
        {
            ClearSlot();
            return;
        }

        if (slot._item._icon != null)
        {
            _iconDisplay.texture = slot._item._icon;
            _iconDisplay.enabled = true;
        }

        if (slot._count > 1)
        {
            _countObject.SetActive(true);
            _countText.text = slot._count.ToString();
        }
        else
        {
            _countObject.SetActive(false);
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (_selectionVisual != null) _selectionVisual.SetActive(isSelected);
    }

    public void ClearSlot()
    {
        _iconDisplay.enabled = false;
        _iconDisplay.texture = null;
        _countObject.SetActive(false);
    }

}
