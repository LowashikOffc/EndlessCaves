using UnityEngine;
using System.IO;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance { get; private set; }

    public Achievements currentAchievements;
    private string saveFilePath;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "achievements.json");
            LoadAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
        AchievementsVisuals.giveAchievement += GiveAchievement;
    }
    public void SaveAchievements()
    {
        string json = JsonUtility.ToJson(currentAchievements, true);

        File.WriteAllText(saveFilePath, json);
        Debug.Log("Achievements saved to: " + saveFilePath);
    }

    private void OnApplicationQuit()
    {
        SaveAchievements();
    }

    public void LoadAchievements()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);

            currentAchievements = JsonUtility.FromJson<Achievements>(json);
            Debug.Log("Achievements loaded.");

        }
        else
        {
            Debug.LogWarning("Achievements file not found. Creating default settings.");
            currentAchievements = new Achievements();
            SaveAchievements();
        }
    }

    private void GiveAchievement(AchievementsEnum achievement)
    {
        if (achievement == AchievementsEnum.Welcome_to_depth) Achievements.Instance.Welcome_to_depth = true;
        if (achievement == AchievementsEnum.Point_of_no_return) Achievements.Instance.Point_of_no_return = true;
        if (achievement == AchievementsEnum.Hooked) Achievements.Instance.Hooked = true;
        if (achievement == AchievementsEnum.Amateur_speleologist) Achievements.Instance.Amateur_speleologist = true;
        if (achievement == AchievementsEnum.Thrifty) Achievements.Instance.Thrifty = true;
        if (achievement == AchievementsEnum.Rock_bottom) Achievements.Instance.Rock_bottom = true;
        if (achievement == AchievementsEnum.Dust_collector) Achievements.Instance.Dust_collector = true;
        SaveAchievements();
    }
}

public enum AchievementsEnum
{
    Welcome_to_depth,
    Point_of_no_return,
    Hooked,
    Amateur_speleologist,
    Thrifty,
    Rock_bottom,
    Dust_collector,
}