using UnityEngine;

public class ItemScanDataScript : MonoBehaviour
{
    public ItemScanData Data;
    public int Weight;
    public bool isScanned = false;

    private void Start()
    {
        Weight = Data.Weight + Random.Range(-30, 30);
    }
}
