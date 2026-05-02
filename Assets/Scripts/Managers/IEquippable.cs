public interface IEquippable
{
    void ExecuteAction(string actionName);
    void OnEquip();
    void OnUnequip();
}
