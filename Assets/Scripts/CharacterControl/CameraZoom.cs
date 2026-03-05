using System;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private int _zoomVal = 70;
    public event Action<int> PlaySound;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _zoomVal = 30;
            PlaySound?.Invoke(5);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            _zoomVal = 70;
            PlaySound?.Invoke(5);
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _zoomVal, Time.deltaTime * 15);
    }
}
