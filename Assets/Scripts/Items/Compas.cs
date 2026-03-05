using UnityEngine;

public class Compas : MonoBehaviour
{
    [SerializeField] Camera _camera;
    private void LateUpdate()
    {
        //transform.position = _camera.transform.position + _camera.transform.up * -0.00085f * _camera.fieldOfView + _camera.transform.forward * 0.00584f * (0.00125f * _camera.fieldOfView);
        transform.localScale = new Vector3(0.000073f, 0.0000875f, 0.000073f) * _camera.fieldOfView;
        transform.rotation = Quaternion.Euler(90, 0, 180);
    }
}
