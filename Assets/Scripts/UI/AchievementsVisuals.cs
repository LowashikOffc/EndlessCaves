using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Panels
{
    public AchievementsEnum _type;
    public TMP_Text _name;
    public TMP_Text _description;
    public Image _value;
    public Image _light;
}

public class AchievementsVisuals : MonoBehaviour
{
    public static AchievementsVisuals instance { get; private set; }
    [SerializeField] private Panels[] _panels;
    [SerializeField] private Color _collectColor;

    public static event Action<AchievementsEnum> giveAchievement;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GiveAchievement(AchievementsEnum achievement)
    {
        giveAchievement?.Invoke(achievement);
        AchievementPanel(achievement);
    }

    private void AchievementPanel(AchievementsEnum achievement)
    {
        Debug.Log($"you get achievement:{achievement}");

        foreach (Panels p in _panels)
        {
            if (p._type == achievement)
            {
                p._value.color = Color.green;
                p._light.color = Color.green;
                p._name.color = _collectColor;
                p._description.color = _collectColor;
            }
        }

    }
}
