using System;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private int _zoomVal = 70;
    private int _zoomSmoothness = 15;
    private int _zoomIn = 30;
    private int _zoomOut = 70; //Settings.Instance._fov
    public event Action<int> PlaySound;

    private void Start()
    {
        InputReceiver.Instance.Zoom += Zoom;
    }
    private void OnDestroy()
    {
        InputReceiver.Instance.Zoom -= Zoom;
    }
    private void Zoom(bool state)
    {
        if (state == true)
        {
            _zoomVal = _zoomIn;
            PlaySound?.Invoke(5);
        }
        else
        {
            _zoomVal = _zoomOut;
            PlaySound?.Invoke(5);
        }
    }
    void Update()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _zoomVal, Time.deltaTime * _zoomSmoothness);
    }
}
