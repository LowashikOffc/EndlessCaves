using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterEffect : MonoBehaviour
{
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up*50, out hit))
        {
            if (hit.collider.CompareTag("Water"))
            {
                RenderSettings.fogColor = new Color(0,0.01f,0.02f);
                RenderSettings.fogEndDistance = 6;
            }
            else if (hit.collider.CompareTag("Lava"))
            {
                RenderSettings.fogColor = new Color(0.5f, 0.2f, 0);
                RenderSettings.fogEndDistance = 3;
            }
            else
            {
                RenderSettings.fogColor = Color.black;
                RenderSettings.fogEndDistance = 20;
            }
        }
    }
}
