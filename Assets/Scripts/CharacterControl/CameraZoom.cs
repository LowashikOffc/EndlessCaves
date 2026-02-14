using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private int _zoomVal = 70;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _zoomVal = 30;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            _zoomVal = 70;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _zoomVal, Time.deltaTime * 15);
    }
}
