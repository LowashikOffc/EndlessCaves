using System.Collections;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    public float batteryEnergy;
    public bool isUsing = false;
    void Start()
    {
        batteryEnergy = 1000;
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (isUsing == true)
            {
                batteryEnergy = batteryEnergy - 1;
            }
        }
    }
}
