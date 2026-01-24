using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NotebookScr : MonoBehaviour
{

    public GameObject pl;
    public GameObject Notebook;
    public GameObject invBar;
    public GameObject null_;
    public GameObject hook;
    private bool isTyping;

    KeyCode inp = 0;

    void LateUpdate()
    {
        int a = 0;
        if (Input.anyKey)
        {
            foreach (GameObject obj in Camera.main.GetComponent<ItemName>().items)
            {
                if (obj.name == Notebook.name)
                {
                    if (a == 0)
                    {
                        inp = UnityEngine.KeyCode.Alpha1;
                    }
                    else if (a == 1)
                    {
                        inp = UnityEngine.KeyCode.Alpha2;
                    }
                    else if (a == 2)
                    {
                        inp = UnityEngine.KeyCode.Alpha3;
                    }
                    else if (a == 3)
                    {
                        inp = UnityEngine.KeyCode.Alpha4;
                    }
                    else if (a == 4)
                    {
                        inp = UnityEngine.KeyCode.Alpha5;
                    }
                    break;
                }
                a++;
            }
        }
        if (Input.GetKeyDown(inp)&&isTyping == false)
        {
            foreach (GameObject obj in Camera.main.GetComponent<ItemName>().items)
            {

                if (obj != Notebook)
                {
                    obj.GetComponent<State>().inHand = false;
                    obj.SetActive(false);
                }
                else if (obj == Notebook && obj.GetComponent<State>().inHand == false)
                {
                    obj.GetComponent<State>().inHand = true;
                    Notebook.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(90, 0, 0);
                    Camera.main.GetComponent<ItemName>().savedHandObj = Notebook;
                    Camera.main.GetComponent<ItemName>().inHandObj = Notebook;
                }
                else if (obj == Notebook && obj.GetComponent<State>().inHand == true)
                {
                    obj.GetComponent<State>().inHand = false;
                    Camera.main.GetComponent<ItemName>().inHandObj = null;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && Camera.main.GetComponent<ItemName>().savedHandObj == Notebook && Notebook.GetComponent<State>().inHand == true&&isTyping == false)
        {
            Notebook.GetComponent<State>().inInv = false;
            Notebook.GetComponent<State>().inHand = false;
            Camera.main.GetComponent<ItemName>().inHandObj = null;
            Camera.main.GetComponent<ItemName>().savedHandObj = null;
            Camera.main.GetComponent<ItemName>().items[a] = null_;
            if (a == 0)
            {
                Destroy(invBar.transform.Find("Slot1").transform.Find("Notebook").gameObject);
                invBar.transform.Find("Slot1").transform.Find("Num").GetComponent<TMP_Text>().color = new Color(0.4f, 0.4f, 0.4f);
            }
            else if (a == 1)
            {
                Destroy(invBar.transform.Find("Slot2").transform.Find("Notebook").gameObject);
                invBar.transform.Find("Slot2").transform.Find("Num").GetComponent<TMP_Text>().color = new Color(0.4f, 0.4f, 0.4f);
            }
            else if (a == 2)
            {
                Destroy(invBar.transform.Find("Slot3").transform.Find("Notebook").gameObject);
                invBar.transform.Find("Slot3").transform.Find("Num").GetComponent<TMP_Text>().color = new Color(0.4f, 0.4f, 0.4f);
            }
            else if (a == 3)
            {
                Destroy(invBar.transform.Find("Slot4").transform.Find("Notebook").gameObject);
                invBar.transform.Find("Slot4").transform.Find("Num").GetComponent<TMP_Text>().color = new Color(0.4f, 0.4f, 0.4f);
            }
            else if (a == 4)
            {
                Destroy(invBar.transform.Find("Slot5").transform.Find("Notebook").gameObject);
                invBar.transform.Find("Slot5").transform.Find("Num").GetComponent<TMP_Text>().color = new Color(0.4f, 0.4f, 0.4f);
            }
            Notebook.GetComponent<Rigidbody>().isKinematic = false;
            Notebook.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);

        }

        if (Input.GetKeyDown(KeyCode.E)&&Camera.main.GetComponent<ItemName>().inHandObj == Notebook)
        {
            isTyping = true;
            pl.GetComponent<CharacterController>().canMove = false;
            pl.GetComponent<CharacterController>().canCrouch = false;
            Camera.main.GetComponent<CameraControl>().canMove = false;
            hook.GetComponent<Graple>().canThrow = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape)&&Camera.main.GetComponent<ItemName>().inHandObj == Notebook)
        {
            isTyping = false;
            pl.GetComponent<CharacterController>().canMove = true;
            pl.GetComponent<CharacterController>().canCrouch = true;
            Camera.main.GetComponent<CameraControl>().canMove = true;
            hook.GetComponent<Graple>().canThrow = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Notebook.GetComponent<State>().inHand == true && Camera.main.GetComponent<ItemName>().inHandObj == Notebook)
        {
            Notebook.SetActive(true);
            Notebook.GetComponent<Rigidbody>().isKinematic = true;
            var t = Camera.main.transform;
            Notebook.transform.position = t.position + t.forward * 0.18f + t.up * (-0.0f) + t.right * 0.075f;
            Notebook.transform.rotation = Quaternion.Lerp(Notebook.transform.rotation, t.rotation * Quaternion.Euler(0, 90, 0), 0.1f);
        }
        else if (Notebook.GetComponent<State>().inHand == false && Camera.main.GetComponent<ItemName>().savedHandObj == Notebook)
        {

            Notebook.SetActive(false);
        }
        else
        {
            Notebook.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

}
