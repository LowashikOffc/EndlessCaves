using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] List<GameObject> slots_;
    [SerializeField] int currentSlot = 0;

    public List<GameObject> slots;
    public GameObject canvas;
    public GameObject currentItem;
    void Start()
    {
        slots_.Add(canvas.transform.Find("InvBarLow").Find("Slot1").gameObject);
        slots_.Add(canvas.transform.Find("InvBarLow").Find("Slot2").gameObject);
        slots_.Add(canvas.transform.Find("InvBarLow").Find("Slot3").gameObject);
        slots_.Add(canvas.transform.Find("InvBarLow").Find("Slot4").gameObject);
        slots_.Add(canvas.transform.Find("InvBarLow").Find("Slot5").gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentSlot == 0) return;
            Interact(currentSlot - 1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentSlot == 0) return;
            DropItem(currentSlot);
            currentSlot = 0;
        }
        if (slots.Count < 1) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PickUpItem(0);
        }
        if (slots.Count < 2) return;
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PickUpItem(1);
        }
        if (slots.Count < 3) return;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PickUpItem(2);
        }
        if (slots.Count < 4) return;
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PickUpItem(3);
        }
        if (slots.Count < 5) return;
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PickUpItem(4);
        }
    }

    public void PickUpItem(int slotNumber)
    {
        currentSlot = slotNumber+1;
        if (slotNumber != currentSlot - 1)
        {
            slots[slotNumber].GetComponent<State>().InInventory();
            return;
        }
        if (slots[slotNumber].GetComponent<State>().inHand == true)
        {
            slots[slotNumber].GetComponent<State>().InInventory();
            return;
        }
        else
        {
            AllInInventory();
            slots[slotNumber].GetComponent<State>().InHand();
        }
        slots[slotNumber].transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(90, 0, 0);
        currentItem = slots[slotNumber];


        UpdateInventory();
    }
    public void DropItem(int slotNumber)
    {
        if (slots[slotNumber - 1].GetComponent<State>().inHand == false) return;
        slots[slotNumber-1].GetComponent<State>().Drop();
        slots.Remove(slots[slotNumber - 1]);


        UpdateInventory();
    }
    public void DestroyItem(int slotNumber)
    {
        GameObject del = slots[slotNumber];
        slots.Remove(slots[slotNumber]);
        Destroy(del);


        UpdateInventory();
    }
    public void AddItem(GameObject item)
    {
        slots.Add(item);
        item.GetComponent<State>().inHand = true;
        item.GetComponent<State>().inInv = true;


        UpdateInventory();
    }

    void AllInInventory()
    {
        foreach(GameObject item in slots)
        {
            item.GetComponent<State>().InInventory();
        }
    }
    void UpdateInventory()
    {
        int i = 0;
        foreach (GameObject slot in slots_)
        {
            slot.transform.Find("ItemName").GetComponent<TMP_Text>().text = null;
        }

        foreach (GameObject item in slots)
        {
            slots_[i].transform.Find("ItemName").GetComponent<TMP_Text>().text = item.name;
            i++;
        }
    }

    void Interact(int slot)
    {
        if (slots[slot].GetComponent<State>().inHand == false) return;
        if (slots[slot].name == "Battery")
        {
            float savedEnergy = slots[slot].GetComponent<BatteryScript>().batteryEnergy;
            slots[slot].GetComponent<BatteryScript>().batteryEnergy = GetComponent<Flashlight>().energy;
            GetComponent<Flashlight>().energy = savedEnergy;
            //DropItem(slot);
            //DestroyItem(currentSlot - 1);
        }
    }
}