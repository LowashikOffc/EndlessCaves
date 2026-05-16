using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlotUI> _uiSlots = new List<InventorySlotUI>();

    private void Start()
    {
        _uiSlots.Clear();
        _uiSlots.AddRange(GetComponentsInChildren<InventorySlotUI>());

        InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();

        InputReceiver.Instance.SlotSelect += UpdateSelectionVisual;
        UpdateSelectionVisual(1);
    }

    private void UpdateSelectionVisual(int slotNumber)
    {
        int selectedIndex = slotNumber - 1;

        for (int i = 0; i < _uiSlots.Count; i++)
        {
            //Debug.Log($"SetSelect{_uiSlots[i]}: {i==selectedIndex}");
            _uiSlots[i].SetSelected(i == selectedIndex);
        }
    }

    public void Refresh()
    {
        if (_uiSlots == null || _uiSlots.Count == 0)
        {
            _uiSlots.Clear();
            _uiSlots.AddRange(GetComponentsInChildren<InventorySlotUI>());
        }

        //Debug.Log($"Найдено ячеек в UI: {_uiSlots.Count}");

        for (int i = 0; i < _uiSlots.Count; i++)
        {
            var slotData = InventoryManager.Instance.GetSlot(i);

            int displayNumber = i + 1;

            _uiSlots[i].UpdateVisual(slotData, displayNumber);
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
            InputReceiver.Instance.SlotSelect -= UpdateSelectionVisual;
        }
    }
}
