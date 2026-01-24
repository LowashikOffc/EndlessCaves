using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SliderScr : MonoBehaviour
{
    public SaveLoadSettings sett;
    public GameObject settObj;

    public AudioMixer mixer;
    public TMP_InputField text;
    private float targetFrameRate;
    private float sliderText;
    float val;
    public PostProcessVolume volume;
    ColorGrading colGr;
    Settings set;

    private string file = "Json.txt";

    private void Start()
    {
        set = new Settings();
        sett = settObj.GetComponent<SaveLoadSettings>();
        set = JsonUtility.FromJson<Settings>(File.ReadAllText(file));
        if (gameObject.name == "SliderFPS")
        {
            gameObject.GetComponent<Slider>().value = set.maxFps;
            if (set.Language == "English")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "FPS limit";
            }
            else if (set.Language == "Русский")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Лимит FPS";
            }
        }
        if (gameObject.name == "SliderBrightness")
        {
            gameObject.GetComponent<Slider>().value = set.Brightness;
            if (set.Language == "English")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Brightness";
            }
            else if (set.Language == "Русский")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Яркость";
            }
        }
        if (gameObject.name == "SliderSmoothing")
        {
            gameObject.GetComponent<Slider>().value = set.Smoothing;
            if (set.Language == "English")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Camera smoothing";
            }
            else if (set.Language == "Русский")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Плавность камеры";
            }
        }
        if (gameObject.name == "SliderSens")
        {
            gameObject.GetComponent<Slider>().value = set.Sens;
            if (set.Language == "English")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Sensetivity";
            }
            else if (set.Language == "Русский")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Скорость камеры";
            }
        }
        if (gameObject.name == "SliderVolume")
        {
            if (set.Language == "English")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Volume";
            }
            else if (set.Language == "Русский")
            {
                gameObject.transform.Find("SliderName").GetComponent<TMP_Text>().text = "Громкость";
            }
            gameObject.GetComponent<Slider>().value = set.Volume;
        }
    }

    public void Update()
    {
        if (gameObject.name == "SliderFPS")
        {
            val = gameObject.GetComponent<Slider>().value;
            targetFrameRate = gameObject.GetComponent<Slider>().value * 60;
            Application.targetFrameRate = (int)targetFrameRate;
            sett.maxFps = val;
            sliderText = gameObject.GetComponent<Slider>().value * 60;

        }
        else if (gameObject.name == "SliderBrightness")
        {
            val = gameObject.GetComponent<Slider>().value*51;
            volume.profile.TryGetSettings(out colGr);
            //colGr.colorFilter.value = new Color32((byte)val, (byte)val, (byte)val, 255);
            sett.Brightness = val / 51;
            sliderText = gameObject.GetComponent<Slider>().value;
        }
        else if (gameObject.name == "SliderSmoothing")
        {
            val = gameObject.GetComponent<Slider>().value;
            sett.Smoothing = val;
            sliderText = gameObject.GetComponent<Slider>().value;
        }
        else if (gameObject.name == "SliderSens")
        {
            val = gameObject.GetComponent<Slider>().value;
            sett.Sens = val;
            sliderText = gameObject.GetComponent<Slider>().value * 10;
        }
        else if (gameObject.name == "SliderVolume")
        {
            val = gameObject.GetComponent<Slider>().value;
            sett.Volume = val;
            mixer.SetFloat("All", Mathf.Log10(val) * 20);
            sliderText = gameObject.GetComponent<Slider>().value;
        }
        text.text = sliderText.ToString();
    }
    
}
