using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class ValueLoad : MonoBehaviour
{
    public bool b1;
    public bool b2;
    public bool b3;
    public bool b4;
    public float maxFps;
    public float Brightness;
    public float Smoothing;
    public float Sens;
    public float Volume;

    private Settings m_Settings;

    public AudioMixer mixer;
    public PostProcessVolume volume;
    ColorGrading colGr;
    void Start()
    {
        
        //brightness
        Brightness = PlayerPrefs.GetFloat("Brightness", Brightness*51);
        volume.profile.TryGetSettings(out colGr);
        colGr.colorFilter.value = new Color32((byte)Brightness, (byte)Brightness, (byte)Brightness, 255);
        Debug.Log(Brightness);

        //FPS
        maxFps = PlayerPrefs.GetFloat("MaxFps", maxFps);
        Debug.Log(maxFps);

        //Camera smooth
        Smoothing = PlayerPrefs.GetFloat("Smoothing", Smoothing);
        Camera.main.GetComponent<CameraControl>().cameraSmoothing = Smoothing;
        Debug.Log(Smoothing);

        //Sensetivity
        //Sens = m_Settings.LoadData().Sens;
        Camera.main.GetComponent<CameraControl>().sensX = Sens*10;
        Camera.main.GetComponent<CameraControl>().sensY = Sens*10;
        Debug.Log(Sens);

        //Volume
        Volume = PlayerPrefs.GetFloat("Sens", Volume);
        mixer.SetFloat("All", Volume);
        Debug.Log(Volume);
    }

    private void Update()
    {
        Application.targetFrameRate = (int)maxFps*60;
    }
}
