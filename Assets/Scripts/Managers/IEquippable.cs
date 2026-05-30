using UnityEngine;

public interface IEquippable
{
    void ExecuteAction(Actions action);
    void OnEquip();
    void OnUnequip();
}

public enum Actions
{
    Primary,
    Secondary,
    WheelUp,
    WheelDown,
}