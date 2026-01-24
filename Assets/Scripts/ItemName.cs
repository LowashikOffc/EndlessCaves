using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemName : MonoBehaviour
{
    public AudioSource snd;
    public GameObject CP;
    public GameObject fl;
    public GameObject invBar;
    public AudioSource snd1;
    public GameObject inHandObj;
    public GameObject savedHandObj;
    public List<GameObject> items;
    public GameObject dos;
    public GameObject Scan;


    public TMP_Text Text;
    public GameObject PUtxt;
    public GameObject pl;
    public LayerMask targetLayer;
    void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.5f, targetLayer))
        {
            if (hit.collider.gameObject.tag == "Collectable" && hit.collider.gameObject.GetComponent<State>().inHand == false)
            {
                Text.text = hit.collider.name;
                if (hit.collider.name == "Battery")
                {
                    Text.text = hit.collider.name + Environment.NewLine + " [" + (hit.collider.GetComponent<BatteryScript>().batteryEnergy / 10).ToString() + "%]";
                }
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    pl.GetComponent<InventoryScript>().AddItem(hit.collider.gameObject);
                    pl.GetComponent<InventoryScript>().PickUpItem(pl.GetComponent<InventoryScript>().slots.Count-1);
                }
                //if (hit.collider.gameObject.GetComponent<State>().inInv == false)
                //{

                //    if (hit.collider.name == "Battery")
                //    {
                //        Text.text = hit.collider.name + " " + hit.collider.GetComponent<BatteryScript>().batteryEnergy / 10 + "%";
                //    }
                //    PUtxt.SetActive(true);
                //    if (b >= 0)
                //    {
                //        if (Input.GetKeyDown(KeyCode.Tab))
                //        {
                //            if (hit.collider.name == "Scanner")
                //            {
                //                Text.text = "Сканер";
                //                GameObject item = Instantiate(hit.collider.gameObject);
                //                item.layer = 13;
                //                foreach (Transform obj in item.transform)
                //                {
                //                    obj.gameObject.layer = 13;
                //                }
                //                item.transform.localScale = item.transform.localScale / 20;
                //                item.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(10, 0, 0);
                //                item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //                item.name = item.name.Replace("(Clone)", "");
                //                hit.collider.GetComponent<State>().inHand = false;
                //                hit.collider.GetComponent<State>().inInv = true;

                //                int num = 0;
                //                int a = 0;
                //                foreach (GameObject obj in items)
                //                {
                //                    if (obj.name == "Null")
                //                    {
                //                        num = a;
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        num = -1;
                //                    }
                //                    a++;
                //                }
                //                if (num >= 0)
                //                {
                //                    items[num] = (hit.collider.gameObject);
                //                }
                //                foreach (GameObject obj in items)
                //                {
                //                    if (obj != Scan)
                //                    {
                //                        obj.GetComponent<State>().inHand = false;
                //                        obj.SetActive(false);
                //                    }
                //                }
                //                savedHandObj = hit.collider.gameObject;
                //                Text.text = null;
                //                PUtxt.SetActive(false);
                //                snd1.Play();
                //                Vector3 addpos = (-Camera.main.transform.forward * 0.001f) + Camera.main.transform.up * (0.001f);
                //                if (num == 0)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot1");
                //                    item.transform.position = invBar.transform.Find("Slot1").transform.position + addpos;
                //                }
                //                else if (num == 1)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot2");
                //                    item.transform.position = invBar.transform.Find("Slot2").transform.position + addpos;
                //                }
                //                else if (num == 2)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot3");
                //                    item.transform.position = invBar.transform.Find("Slot3").transform.position + addpos;
                //                }
                //                else if (num == 3)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot4");
                //                    item.transform.position = invBar.transform.Find("Slot4").transform.position + addpos;
                //                }
                //                else if (num == 4)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot5");
                //                    item.transform.position = invBar.transform.Find("Slot5").transform.position + addpos;
                //                }
                //            }

                //            else if (hit.collider.name == "Dosimeter")
                //            {
                //                Text.text = "Дозиметр";
                //                GameObject item = Instantiate(hit.collider.gameObject);
                //                item.layer = 13;
                //                foreach (Transform obj in item.transform)
                //                {
                //                    obj.gameObject.layer = 13;
                //                }
                //                item.transform.localScale = item.transform.localScale / 30;
                //                item.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(10, 100, 0);
                //                item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //                item.name = item.name.Replace("(Clone)", "");
                //                hit.collider.GetComponent<State>().inHand = false;
                //                hit.collider.GetComponent<State>().inInv = true;

                //                int num = 0;
                //                int a = 0;
                //                foreach (GameObject obj in items)
                //                {
                //                    if (obj.name == "Null")
                //                    {
                //                        num = a;
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        num = -1;
                //                    }
                //                    a++;
                //                }
                //                if (num >= 0)
                //                {
                //                    items[num] = (hit.collider.gameObject);
                //                }
                //                foreach (GameObject obj in items)
                //                {
                //                    if (obj != dos)
                //                    {
                //                        obj.GetComponent<State>().inHand = false;
                //                        obj.SetActive(false);
                //                    }
                //                }

                //                savedHandObj = hit.collider.gameObject;
                //                Text.text = null;
                //                PUtxt.SetActive(false);
                //                snd1.Play();
                //                Vector3 addpos = (-Camera.main.transform.forward * 0.001f) + Camera.main.transform.up * (0.001f);
                //                if (num == 0)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot1");
                //                    item.transform.position = invBar.transform.Find("Slot1").transform.position + addpos;
                //                }
                //                else if (num == 1)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot2");
                //                    item.transform.position = invBar.transform.Find("Slot2").transform.position + addpos;
                //                }
                //                else if (num == 2)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot3");
                //                    item.transform.position = invBar.transform.Find("Slot3").transform.position + addpos;
                //                }
                //                else if (num == 3)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot4");
                //                    item.transform.position = invBar.transform.Find("Slot4").transform.position + addpos;
                //                }
                //                else if (num == 4)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot5");
                //                    item.transform.position = invBar.transform.Find("Slot5").transform.position + addpos;
                //                }
                //            }

                //            else if (hit.collider.name == "Notebook")
                //            {
                //                Text.text = "Блокнот";
                //                GameObject item = Instantiate(hit.collider.gameObject);
                //                item.layer = 13;
                //                foreach (Transform obj in item.transform)
                //                {
                //                    obj.gameObject.layer = 13;
                //                }
                //                item.transform.localScale = item.transform.localScale / 28;
                //                item.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(10, 100, 0);
                //                item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                //                item.name = item.name.Replace("(Clone)", "");
                //                hit.collider.GetComponent<State>().inHand = false;
                //                hit.collider.GetComponent<State>().inInv = true;

                //                int num = 0;
                //                int a = 0;
                //                foreach (GameObject obj in items)
                //                {
                //                    if (obj.name == "Null")
                //                    {
                //                        num = a;
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        num = -1;
                //                    }
                //                    a++;
                //                }
                //                if (num >= 0)
                //                {
                //                    items[num] = (hit.collider.gameObject);
                //                }
                //                foreach (GameObject obj in items)
                //                {
                //                    if (obj != dos)
                //                    {
                //                        obj.GetComponent<State>().inHand = false;
                //                        obj.SetActive(false);
                //                    }
                //                }

                //                savedHandObj = hit.collider.gameObject;
                //                Text.text = null;
                //                PUtxt.SetActive(false);
                //                snd1.Play();
                //                Vector3 addpos = (-Camera.main.transform.forward * 0.0025f) + Camera.main.transform.right * 0.003f;
                //                if (num == 0)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot1");
                //                    item.transform.position = invBar.transform.Find("Slot1").transform.position + addpos;
                //                }
                //                else if (num == 1)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot2");
                //                    item.transform.position = invBar.transform.Find("Slot2").transform.position + addpos;
                //                }
                //                else if (num == 2)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot3");
                //                    item.transform.position = invBar.transform.Find("Slot3").transform.position + addpos;
                //                }
                //                else if (num == 3)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot4");
                //                    item.transform.position = invBar.transform.Find("Slot4").transform.position + addpos;
                //                }
                //                else if (num == 4)
                //                {
                //                    item.transform.parent = invBar.transform.Find("Slot5");
                //                    item.transform.position = invBar.transform.Find("Slot5").transform.position + addpos;
                //                }
                //            }

                //            else if (hit.collider.name == "Battery")
                //            {
                //                hit.collider.transform.Find("BatteryChange").GetComponent<AudioSource>().Play();
                //                hit.collider.gameObject.transform.position = pl.transform.position;
                //                float savedEnergy = pl.GetComponent<Flashlight>().energy;
                //                pl.GetComponent<Flashlight>().energy = hit.collider.GetComponent<BatteryScript>().batteryEnergy;
                //                if (hit.collider.GetComponent<BatteryScript>().batteryEnergy > 0)
                //                {
                //                    pl.GetComponent<Flashlight>().CanEn = true;
                //                }
                //                hit.collider.GetComponent<BatteryScript>().batteryEnergy = savedEnergy;
                //                print(hit.collider.GetComponent<BatteryScript>().batteryEnergy);
                //                hit.collider.GetComponent<BatteryScript>().isUsing = false;
                //            }
                //        }
                //    }
                //}
            }
            else
            {
            Text.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            PUtxt.SetActive(false);
        }
    }
        else
        {
            Text.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            PUtxt.SetActive(false);
        }
    }
}
