using UnityEngine;

public class Compas : MonoBehaviour
{
    [SerializeField] Camera _camera;
    private void LateUpdate()
    {
        transform.localScale = new Vector3(0.0000875f, 0.0000875f, 0.0000875f) * _camera.fieldOfView;
        transform.rotation = Quaternion.Euler(90, 0, 180);
    }
}
