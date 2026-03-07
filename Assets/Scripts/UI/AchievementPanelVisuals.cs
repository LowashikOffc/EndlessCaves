using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanelVisuals : MonoBehaviour
{
    [SerializeField] private Color _collectColor;
    [SerializeField] private AchievementsEnum _type;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _value;
    [SerializeField] private Image _light;

    private void Start()
    {
        if (AchievementManager.instance != null)
        {
            AchievementManager.instance.LoadAchievement += AchievementPanel;
            Debug.Log("Connected to LoadAchievement");
        }
    }

    private void GiveAchievement(AchievementsEnum achievement)
    {
        AchievementPanel(achievement);
        Debug.Log($"you get achievement:{achievement}");
    }

    private void AchievementPanel(AchievementsEnum achievement)
    {
        Debug.Log("Work!");
        if (_type == achievement)
        {
            Debug.Log(2);
            Debug.Log($"{_name} added");
            _value.color = Color.green;
            _light.color = new Color32(0,255,0,20);
            _name.color = _collectColor;
            _description.color = _collectColor;
        }

    }
}
