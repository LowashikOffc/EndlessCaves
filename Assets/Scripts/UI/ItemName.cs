using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemName : MonoBehaviour
{
    [SerializeField] private TMP_Text _displayText;
    [SerializeField] private GameObject _pickUpText;
    [SerializeField] private GameObject _player;
    [SerializeField] private LayerMask _targetLayer;

    private void Start()
    {

    }
    void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.5f, _targetLayer))
        {
            if (hit.collider.gameObject.tag == "Collectable" && hit.collider.gameObject.GetComponent<State>().inHand == false)
            {
                _displayText.text = hit.collider.name;
                if (hit.collider.name == "Battery")
                {
                    _displayText.text = hit.collider.name + Environment.NewLine + " [" + (hit.collider.GetComponent<BatteryScript>().batteryEnergy / 10).ToString() + "%]";
                }
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    _player.GetComponent<InventoryScript>().AddItem(hit.collider.gameObject);
                    _player.GetComponent<InventoryScript>().PickUpItem(_player.GetComponent<InventoryScript>().slots.Count-1);
                }
            }
            else
            {
            _displayText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            _pickUpText.SetActive(false);
        }
    }
        else
        {
            _displayText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            _pickUpText.SetActive(false);
        }
    }
}
