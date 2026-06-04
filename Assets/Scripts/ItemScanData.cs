using UnityEngine;

[CreateAssetMenu(fileName = "ScanData", menuName = "Scan/Data")]
public class ItemScanData : ScriptableObject
{
    public string Name;
    public string Description;
    public int Weight;
    public Rare Rare;
    public int Cost;
}

public enum Rare
{
    Common,
    Rare,
    Very_Rare,
}
