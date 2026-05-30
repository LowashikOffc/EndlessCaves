using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    public Settings currentSettings;
    private string saveFilePath;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "gamesettings.json");
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(currentSettings, true);

        File.WriteAllText(saveFilePath, json);
        //Debug.Log("Settings saved to: " + saveFilePath);
    }

    public void LoadSettings()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);

            currentSettings = JsonUtility.FromJson<Settings>(json);
            //Debug.Log("Settings loaded.");

        }
        else
        {
            //Debug.LogWarning("Settings file not found. Creating default settings.");
            currentSettings = new Settings();
            SaveSettings();
        }
    }
    public void ButtonChange(settingsEnum _enum, bool value)
    {
        switch (_enum)
        {
            case settingsEnum.invertMouseX: Settings.Instance._invertMouseX = value; break;
            case settingsEnum.invertMouseY: Settings.Instance._invertMouseY = value; break;
            case settingsEnum.vignette: Settings.Instance._vignette = value; break;
            case settingsEnum.grain: Settings.Instance._grain = value; break;
            case settingsEnum.ambientOcclusion: Settings.Instance._ambientOcclusion = value; break;
            case settingsEnum.vSync: Settings.Instance._vSync = value; break;
        }
        SaveSettings();
    }
    public void SliderChange(settingsEnum _enum, byte value)
    {
        switch (_enum)
        {
            case settingsEnum.mouseSensitivity: Settings.Instance._mouseSensitivity = value; break;
            case settingsEnum.gamma: Settings.Instance._gamma = value; break;
            case settingsEnum.maxFps: Settings.Instance._maxFps = value; break;
            case settingsEnum.viewDistance: Settings.Instance._viewDistance = value; break;
            case settingsEnum.shadowsQuality: Settings.Instance._shadowsQuality = value; break;
            case settingsEnum.particleQuality: Settings.Instance._particleQuality = value; break;
            case settingsEnum.TextureQuality: Settings.Instance._TextureQuality = value; break;
            case settingsEnum.masterVolume: Settings.Instance._masterVolume = value; break;
            case settingsEnum.ambient: Settings.Instance._ambient = value; break;
            case settingsEnum.music: Settings.Instance._music = value; break;
            case settingsEnum.guiSounds: Settings.Instance._guiSounds = value; break;
            case settingsEnum.scale: Settings.Instance._scale = value; break;
        }
        SaveSettings();
    }
    public void KeyCodeChange(settingsEnum _enum, KeyCode value)
    {
        InputReceiver.Instance.Rebind(_enum, value);
    }
    public void DropBoxChange(settingsEnum _enum, string value)
    {

    }
}

public enum settingsEnum
{
    mouseSensitivity,
    invertMouseX,
    invertMouseY,

    frontKey,
    leftKey,
    rightKey,
    backKey,
    jumpKey,
    sprintKey,
    crouchKey,
    flashlightKey,
    zoomKey,
    actionKey,
    dropKey,

    vignette,
    grain,
    ambientOcclusion,
    gamma,
    vSync,
    maxFps,
    viewDistance,
    shadowsQuality,
    particleQuality,
    TextureQuality,
    antiAliasing,

    masterVolume,
    ambient,
    music,
    guiSounds,

    scale,
    language
}
