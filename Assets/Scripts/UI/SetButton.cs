using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SetButton : MonoBehaviour
{
    bool active = false;
    public AudioSource snd;
    public PostProcessVolume volume;
    public SaveLoadSettings sett;
    public GameObject settObj;
    public PSXEffects psx;
    Bloom bloom;
    ChromaticAberration chrom;
    Settings set;

    private string file = "Json.txt";

    private void Start()
    {
        set = new Settings();
        psx = Camera.main.GetComponent<PSXEffects>();
        sett = settObj.GetComponent<SaveLoadSettings>();
        set = JsonUtility.FromJson<Settings>(File.ReadAllText(file));
        if (gameObject.name == "Button1")
        {
            active = set.b1;
            if (active == true)
            {
                volume.profile.TryGetSettings<Bloom>(out bloom);
                bloom.active = true;
            }
            else
            {
                volume.profile.TryGetSettings<Bloom>(out bloom);
                bloom.active = false;
            }
        }
        else if (gameObject.name == "Button2")
        {
            active = set.b2;
            if (active == true)
            {
                Camera.main.GetComponent<PSXEffects>().affineMapping = true;
            }
            else
            {
                Camera.main.GetComponent<PSXEffects>().affineMapping = false;
            }
        }
        else if (gameObject.name == "Button3")
        {
            active = set.b3;
        }
        else if (gameObject.name == "Button4")
        {
            active = set.b4;
            if (active == true)
            {
                volume.profile.TryGetSettings<ChromaticAberration>(out chrom);
                chrom.active = true;
            }
            else
            {
                volume.profile.TryGetSettings<ChromaticAberration>(out chrom);
                chrom.active = false;
            }
        }
        if (active == true)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 150, 50, 255);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
    }
    public void onClick()
    {
        if (active == false)
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 150, 50, 255);
            active = true;
            snd.Play();
            if (gameObject.name == "Button1")
            {
                settObj.GetComponent<SaveLoadSettings>().b1 = true;
                volume.profile.TryGetSettings<Bloom>(out bloom);
                bloom.active = true;
            }
            else if (gameObject.name == "Button2")
            {
                settObj.GetComponent<SaveLoadSettings>().b2 = true;
                psx.affineMapping = true;
            }
            else if (gameObject.name == "Button3")
            {
                settObj.GetComponent<SaveLoadSettings>().b3 = true;
            }
            else if (gameObject.name == "Button4")
            {
                settObj.GetComponent<SaveLoadSettings>().b4 = true;
                volume.profile.TryGetSettings<ChromaticAberration>(out chrom);
                chrom.active = true;
            }
        }
        else if (active == true)
        {
            gameObject.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
            active = false;
            snd.Play();
            if (gameObject.name == "Button1")
            {
                settObj.GetComponent<SaveLoadSettings>().b1 = false;
                volume.profile.TryGetSettings<Bloom>(out bloom);
                bloom.active = false;
            }
            else if (gameObject.name == "Button2")
            {
                settObj.GetComponent<SaveLoadSettings>().b2 = false;
                psx.affineMapping = false;
            }
            else if (gameObject.name == "Button3")
            {
                settObj.GetComponent<SaveLoadSettings>().b3 = false;
            }
            else if (gameObject.name == "Button4")
            {
                settObj.GetComponent<SaveLoadSettings>().b4 = false;
                volume.profile.TryGetSettings<ChromaticAberration>(out chrom);
                chrom.active = false;
            }
        }
    }
}
