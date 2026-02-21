using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class SaveLoadSettings : MonoBehaviour
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
    public string language;

    public AudioMixer mixer;
    public PostProcessVolume volume;
    ColorGrading colGr;

    Settings set;

    private string file = "Json.txt";


    public void Save()
    {
        Settings set = new Settings();
        set.b1 = b1;
        set.b2 = b2;
        set.b3 = b3;
        set.b4 = b4;
        set.maxFps = maxFps;
        set.Brightness = Brightness;
        set.Smoothing = Smoothing;
        set.Sens = Sens;
        set.Volume = Volume;
        set.Language = language;
        string json = JsonUtility.ToJson(set);
        File.WriteAllText(file, json);
        Debug.Log(set.maxFps + " " + set.Brightness + " " + set.Smoothing + " " + set.Sens);
    }

    public void load()
    {
        set = JsonUtility.FromJson<Settings>(File.ReadAllText(file));
        Debug.Log(set.maxFps + " " + set.Brightness + " " + set.Smoothing + " " + set.Sens);

        //Sounds
        //mixer.SetFloat("All", Mathf.Log10(Volume) * 20);

        //Brightness
        volume.profile.TryGetSettings(out colGr);
        float br = set.Brightness * 51;
        //colGr.colorFilter.value = new Color32((byte)br, (byte)br, (byte)br, 255);

        //Sensetivity
        if (SceneManager.GetActiveScene().name == "Ingame")
        {
            Camera.main.GetComponent<CameraControl>().sensX = set.Sens * 10;
            Camera.main.GetComponent<CameraControl>().sensY = set.Sens * 10;
        }

        //Smoothing
        if (SceneManager.GetActiveScene().name == "Ingame")
        {
            Camera.main.GetComponent<CameraControl>().cameraSmoothing = set.Smoothing;
        }
    }

    private void Start()
    {
        
        set = JsonUtility.FromJson<Settings>(File.ReadAllText(file));
        b1 = set.b1;
        b2 = set.b2;
        b3 = set.b3;
        b4 = set.b4;
        maxFps = set.maxFps;
        Brightness = set.Brightness;
        Smoothing = set.Smoothing;
        Sens = set.Sens;
        Volume = set.Volume;
        language = set.Language;
        
        load();
    }
}
