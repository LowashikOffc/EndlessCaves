using UnityEngine;

public class ButtonInput : MonoBehaviour
{
    [SerializeField] private settingsEnum _settingsEnum;
    [SerializeField] private bool _value;

    public void OnChanged()
    {
        SettingsManager.instance.ButtonChange(_settingsEnum, _value);
    }
}
