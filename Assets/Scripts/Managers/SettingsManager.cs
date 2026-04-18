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
        bool currentValue;
        switch (_enum)
        {
            case settingsEnum.invertMouseX:
                currentValue = Settings.Instance._invertMouseX;
                break;

            case settingsEnum.invertMouseY:
                currentValue = Settings.Instance._invertMouseY;
                break;

            case settingsEnum.vignette:
                currentValue = Settings.Instance._vignette;
                break;

            case settingsEnum.grain:
                currentValue = Settings.Instance._grain;
                break;

            case settingsEnum.ambientOcclusion:
                currentValue = Settings.Instance._ambientOcclusion;
                break;

            case settingsEnum.vSync:
                currentValue = Settings.Instance._vSync;
                break;
        }
        currentValue = value;
    }
    public void SliderChange(settingsEnum _enum, byte value)
    {
        byte currentValue;
        switch (_enum)
        {
            case settingsEnum.mouseSensitivity:
                currentValue = Settings.Instance._mouseSensitivity;
                break;

            case settingsEnum.gamma:
                currentValue = Settings.Instance._gamma;
                break;

            case settingsEnum.maxFps:
                currentValue = Settings.Instance._maxFps;
                break;

            case settingsEnum.viewDistance:
                currentValue = Settings.Instance._viewDistance;
                break;

            case settingsEnum.shadowsQuality:
                currentValue = Settings.Instance._shadowsQuality;
                break;

            case settingsEnum.particleQuality:
                currentValue = Settings.Instance._particleQuality;
                break;

            case settingsEnum.TextureQuality:
                currentValue = Settings.Instance._TextureQuality;
                break;

            case settingsEnum.masterVolume:
                currentValue = Settings.Instance._masterVolume;
                break;

            case settingsEnum.ambient:
                currentValue = Settings.Instance._ambient;
                break;

            case settingsEnum.music:
                currentValue = Settings.Instance._music;
                break;

            case settingsEnum.guiSounds:
                currentValue = Settings.Instance._guiSounds;
                break;

            case settingsEnum.scale:
                currentValue = Settings.Instance._scale;
                break;
        }

        currentValue = value;
    }
    public void KeyCodeChange(settingsEnum _enum, KeyCode value)
    {

        switch (_enum)
        {
            case settingsEnum.frontKey:
                Settings.Instance._front = value;
                break;
            case settingsEnum.leftKey:
                Settings.Instance._left = value;
                break;
            case settingsEnum.backKey:
                Settings.Instance._back = value;
                break;
            case settingsEnum.rightKey:
                Settings.Instance._right = value;
                break;
            case settingsEnum.jumpKey:
                Settings.Instance._jump = value;
                break;
            case settingsEnum.crouchKey:
                Settings.Instance._crouch = value;
                break;
            case settingsEnum.sprintKey:
                Settings.Instance._sprint = value;
                break;
            case settingsEnum.flashlightKey:
                Settings.Instance._flashlight = value;
                break;
            case settingsEnum.zoomKey:
                Settings.Instance._zoom = value;
                break;
            case settingsEnum.actionKey:
                Settings.Instance._action = value;
                break;
            case settingsEnum.dropKey:
                Settings.Instance._drop = value;
                break;
        }

        SaveSettings();  // ✅ Не забудьте сохранить в файл!
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
