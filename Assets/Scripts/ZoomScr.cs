using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomScr : MonoBehaviour
{
    private int zoomVal = 70;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            zoomVal = 30;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            zoomVal = 70;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomVal, 0.1f);
    }
}
