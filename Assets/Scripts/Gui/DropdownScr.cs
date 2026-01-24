using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownScr : MonoBehaviour
{
    public SaveLoadSettings sett;
    Settings set;
    public GameObject settObj;

    private string file = "Json.txt";

    string language = "En";

    public void Start()
    {
        set = new Settings();
        sett = settObj.GetComponent<SaveLoadSettings>();
        set = JsonUtility.FromJson<Settings>(File.ReadAllText(file));
        print(set.Language);
        if (set.Language == "English")
        {
            gameObject.transform.Find("Name").GetComponent<TMP_Text>().text = "Language";
            print(GetComponent<TMP_Dropdown>().value);
            GetComponent<TMP_Dropdown>().value = 0;
        }
        if (set.Language == "–усский")
        {
            gameObject.transform.Find("Name").GetComponent<TMP_Text>().text = "язык";
            print(GetComponent<TMP_Dropdown>().value);
            GetComponent<TMP_Dropdown>().value = 1;
        }
    }
    public void onUpdate()
    {
        set = new Settings();
        sett = settObj.GetComponent<SaveLoadSettings>();
        set = JsonUtility.FromJson<Settings>(File.ReadAllText(file));
        string txt = gameObject.transform.Find("Label").GetComponent<TMP_Text>().text;
        sett.language = txt;
        print(set.Language);
    }
}
