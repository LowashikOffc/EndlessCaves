using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ValueInput : MonoBehaviour
{
    public GameObject slider;

    public PostProcessVolume volume;
    ColorGrading colGr;

    private void Start()
    {
        slider = transform.parent.gameObject;
    }
    public void onEdited()
    {
        string val = gameObject.GetComponent<TMP_InputField>().text;
        slider.GetComponent<UnityEngine.UI.Slider>().value = float.Parse(val);
        print(val);
        if (slider.name == "SliderFPS")
        {
            Application.targetFrameRate = (int)slider.GetComponent<UnityEngine.UI.Slider>().value;
        }
        if (slider.name == "Brightness")
        {
            volume.profile.TryGetSettings(out colGr);
            colGr.colorFilter.value = new Color32((byte)slider.GetComponent<UnityEngine.UI.Slider>().value, (byte)slider.GetComponent<UnityEngine.UI.Slider>().value, (byte)slider.GetComponent<UnityEngine.UI.Slider>().value, 255);
        }
        if (slider.name == "Smoothing")
        {
            volume.profile.TryGetSettings(out colGr);
            colGr.colorFilter.value = new Color32((byte)slider.GetComponent<UnityEngine.UI.Slider>().value, (byte)slider.GetComponent<UnityEngine.UI.Slider>().value, (byte)slider.GetComponent<UnityEngine.UI.Slider>().value, 255);
        }
    }
}
