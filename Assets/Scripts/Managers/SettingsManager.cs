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
}
    