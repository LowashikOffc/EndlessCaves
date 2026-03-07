using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance { get; private set; }

    public Achievements _currentAchievements;
    private string saveFilePath;

    public event Action<AchievementsEnum> GiveAchievement;
    public event Action<AchievementsEnum> LoadAchievement;

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
    }

    public void SaveAchievements()
    {
        string json = JsonUtility.ToJson(_currentAchievements, true);

        File.WriteAllText(saveFilePath, json);
        //Debug.Log("Achievements saved to: " + saveFilePath);
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

            _currentAchievements = JsonUtility.FromJson<Achievements>(json);
            //Debug.Log("Achievements loaded.");
            StartCoroutine(waitForLoad());
        }
        else
        {
            //Debug.LogWarning("Achievements file not found. Creating default settings.");
            _currentAchievements = new Achievements();
            SaveAchievements();
        }
    }

    IEnumerator waitForLoad()
    {
        yield return new WaitForSeconds(1);
        LoadReceivedAchievements();
    }
    private void LoadReceivedAchievements()
    {
        if (_currentAchievements.Welcome_to_depth == true) LoadAchievement?.Invoke(AchievementsEnum.Welcome_to_depth);
        if (_currentAchievements.Point_of_no_return == true) LoadAchievement?.Invoke(AchievementsEnum.Point_of_no_return);
        if (_currentAchievements.Hooked == true) LoadAchievement?.Invoke(AchievementsEnum.Hooked);
        if (_currentAchievements.Amateur_speleologist == true) LoadAchievement?.Invoke(AchievementsEnum.Amateur_speleologist);
        if (_currentAchievements.Thrifty == true) LoadAchievement?.Invoke(AchievementsEnum.Thrifty);
        if (_currentAchievements.Rock_bottom == true) LoadAchievement?.Invoke(AchievementsEnum.Rock_bottom);
        if (_currentAchievements.Dust_collector == true) LoadAchievement?.Invoke(AchievementsEnum.Dust_collector);
        Debug.Log(_currentAchievements.Welcome_to_depth);
    }

    private void NewAchievement(AchievementsEnum achievement)
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