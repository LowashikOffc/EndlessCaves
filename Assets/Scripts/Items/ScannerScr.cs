using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScannerScr : MonoBehaviour
{
    private bool canScan = true;
    public GameObject Scanner;
    public GameObject RayPos;
    public TMP_Text txt;
    public TMP_Text Infotxt;
    public TMP_Text Scantxt;
    public TMP_Text discSpaceTxt;
    public LayerMask targetLayer;
    public Image ScanImg;

    string language = "Ru";

    public AudioSource equip;
    public AudioSource startScan;
    public AudioSource endScan;
    public AudioSource scanningSnd;
    public AudioSource error;

    Coroutine last;
    private float maxSpace = 100;
    public float discSpace = 0;

    public GameObject targetObject;

    Settings set;
    public GameObject settObj;
    private string file = "Json.txt";

    Quaternion saveRot;
    private void Start()
    {

        discSpaceTxt.text = discSpace + "/" + maxSpace + "kb";
        //set = new Settings();
        //sett = settObj.GetComponent<SaveLoadSettings>();
        //set = JsonUtility.FromJson<Settings>(File.ReadAllText(file));
        //if (set.Language == "English")
        //{
        //    language = "En";
        //}
        //else if (set.Language == "Русский")
        //{
        //    language = "Ru";
        //}
    }
    void Update()
    {
        //if (GetComponent<InventoryScript>().currentItem != Scanner) return;
        RaycastHit hit2;
        if (Physics.Raycast(RayPos.transform.position, RayPos.transform.forward * 2, out hit2, 0.5f, targetLayer))
        {
            targetObject = hit2.collider.gameObject;
        }
        else
        {
            if (canScan == false)
            {
                Error();
            }

        }
        if (Input.GetKey(KeyCode.E) && canScan == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(RayPos.transform.position, RayPos.transform.forward * 2, out hit, 0.5f, targetLayer))
            {
                if (hit.collider != null && hit.collider.name != "Player" && hit.collider.name != "Cube")
                {
                    last = StartCoroutine(scanning());
                    IEnumerator scanning()
                    {
                        Vector3 pos = new Vector3(Mathf.Floor(hit.point.x), Mathf.Floor(hit.point.y), Mathf.Floor(hit.point.z));
                        string name = "???";
                        float ScanTime = 0.1f;
                        float size = 1;
                        string sizeSys = "kb";

                        if (hit.collider.gameObject.name == "Stone")
                        {
                            name = "Камень";
                            ScanTime = 0.2f;
                        }
                        else if (hit.collider.gameObject.name == "Basalt")
                        {
                            name = "Базальт";
                            ScanTime = 0.15f;
                        }
                        else if (hit.collider.gameObject.name == "Diorite")
                        {
                            name = "Диорит";
                            ScanTime = 0.2f;
                        }
                        else if (hit.collider.gameObject.name == "Diabas")
                        {
                            name = "Диабаз";
                            ScanTime = 0.2f;
                        }
                        else if (hit.collider.gameObject.name == "Granite")
                        {
                            name = "Гранит";
                            ScanTime = 0.15f;
                        }
                        else if (hit.collider.gameObject.name == "Radioactive crystal")
                        {
                            name = "Радиоактивный кристалл";
                            ScanTime = 0.05f;
                        }
                        else if (hit.collider.gameObject.name == "Amethyst")
                        {
                            name = "Аметист";
                            ScanTime = 0.08f;
                        }
                        else if (hit.collider.gameObject.name == "Quartz")
                        {
                            name = "Кварц";
                            ScanTime = 0.08f;
                        }
                        else if (hit.collider.gameObject.name == "Stalagmite" || hit.collider.gameObject.name == "Stalagtite")
                        {
                            name = "Камень";
                            ScanTime = 0.15f;
                        }


                        startScan.Play();
                        GameObject obj = hit.collider.gameObject;
                        txt.text = "";
                        Infotxt.text = "";
                        canScan = false;
                        Scantxt.color = new Color(1, 1, 0, 0.5f);
                        if (language == "En")
                        {
                            Scantxt.fontSize = 0.2f;
                            name = hit.collider.name;
                            Scantxt.text = "SCANNING";
                        }
                        else if (language == "Ru")
                        {
                            Scantxt.fontSize = 0.15f;
                            Scantxt.text = "СКАНИРОВАНИЕ";
                        }
                        ScanImg.rectTransform.sizeDelta = new Vector2(0, 0.05f);
                        ScanImg.gameObject.SetActive(true);
                        scanningSnd.volume = 0.4f;
                        for (float i = 0; i < 0.8f; i += ScanTime)
                        {
                            if (canScan == true)
                            {
                                ScanImg.rectTransform.sizeDelta = new Vector2(0, 0.05f);
                                break;
                            }
                            else
                            {
                                float rand = UnityEngine.Random.Range(0.3f, 1f);
                                yield return new WaitForSeconds(rand);
                                StartCoroutine(a());
                                IEnumerator a()
                                {
                                    float rand = UnityEngine.Random.Range(0.7f, 1.5f);
                                    scanningSnd.pitch = rand;
                                    scanningSnd.Play();
                                    while (canScan == false)
                                    {
                                        ScanImg.rectTransform.sizeDelta = Vector2.Lerp(ScanImg.rectTransform.sizeDelta, new Vector2(i, 0.05f), 0.01f);
                                        yield return new WaitForEndOfFrame();
                                    }
                                }
                            }
                        }
                        if (targetObject != obj)
                        {
                            if (canScan == false)
                            {
                                Error();
                            }
                        }
                        else
                        {
                            if (canScan == false)
                            {
                                scanningSnd.Stop();
                                endScan.Play();
                                Scantxt.color = new Color(0, 1, 0, 0.5f);
                                Scantxt.fontSize = 0.2f;
                                if (language == "En")
                                {
                                    Scantxt.text = "SUCCESFUL";
                                }
                                else if (language == "Ru")
                                {
                                    Scantxt.text = "УСПЕШНО";
                                }
                                ScanImg.gameObject.SetActive(false);
                                if (language == "En")
                                {
                                    Infotxt.text = "Info";
                                }
                                else if (language == "Ru")
                                {
                                    Infotxt.text = "Информация";
                                }


                                size = MathF.Pow(ScanTime, UnityEngine.Random.Range(-2.2f, -2.3f));
                                size = MathF.Floor(size * 10) / 100;
                                if (discSpace + size < maxSpace)
                                {
                                    discSpace += size;
                                    discSpaceTxt.text = discSpace + "/" + maxSpace + "kb";

                                    if (language == "En")
                                    {
                                        txt.text = "Object" + Environment.NewLine

                                        + name + Environment.NewLine + Environment.NewLine


                                        + "Position" + Environment.NewLine

                                        + "X: " + pos.x + "    Y: " + pos.y + "    Z: " + pos.z + Environment.NewLine + Environment.NewLine

                                        + "File size" + Environment.NewLine

                                        + size + sizeSys + Environment.NewLine + Environment.NewLine

                                        + "recorded in" + Environment.NewLine

                                        + System.DateTime.Now;
                                    }
                                    else if (language == "Ru")
                                    {
                                        txt.text = "ОбЪект" + Environment.NewLine

                                        + name + Environment.NewLine + Environment.NewLine


                                        + "Позиция" + Environment.NewLine

                                        + "X: " + pos.x + "    Y: " + pos.y + "    Z: " + pos.z + Environment.NewLine + Environment.NewLine

                                        + "Размер файла" + Environment.NewLine

                                        + size + sizeSys + Environment.NewLine + Environment.NewLine

                                        + "Запись" + Environment.NewLine

                                        + System.DateTime.Now;

                                    }
                                    yield return new WaitForSeconds(0.5f);
                                    canScan = true;
                                    Scantxt.text = "";
                                }
                                else
                                {

                                    NESpace();
                                }
                            }

                        }
                    }
                }
            }
        }
    }

    private void Error()
    {
        StopCoroutine(last);
        scanningSnd.Stop();
        scanningSnd.volume = 0;
        targetObject = null;
        canScan = true;
        Scantxt.fontSize = 0.17f;
        if (language == "En")
        {
            Scantxt.text = "INTERRUPTED";
        }
        else if (language == "Ru")
        {
            Scantxt.text = "ПРЕРВАНО";
        }
        error.Play();
        ScanImg.rectTransform.sizeDelta = new Vector2(0, 0.05f);
        Scantxt.color = new Color(1, 0, 0, 0.5f);
        saveRot = new Quaternion(0, 0, 1, 0);
    }

    private void NESpace()
    {
        StopCoroutine(last);
        scanningSnd.Stop();
        scanningSnd.volume = 0;
        targetObject = null;
        canScan = true;
        Scantxt.fontSize = 0.16f;
        if (language == "En")
        {
            Scantxt.text = "NOT ENOUGH SPACE";
        }
        else if (language == "Ru")
        {
            Scantxt.text = "НЕДОСТАТОЧНО МЕСТА";
        }
        error.Play();
        ScanImg.rectTransform.sizeDelta = new Vector2(0, 0.05f);
        Scantxt.color = new Color(1, 0, 0, 0.5f);
        saveRot = new Quaternion(0, 0, 1, 0);
    }
}
