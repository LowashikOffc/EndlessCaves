using UnityEngine;

public class HandMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _additionalPosition;
    [SerializeField] private Quaternion _additionalRotation;
    private Camera _camera;
    private Vector3 newPos = Vector3.zero;
    private Quaternion newRot = Quaternion.identity;
    void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        newPos = _camera.transform.position
            + _camera.transform.right * _additionalPosition.x
            + _camera.transform.up * _additionalPosition.y
            + _camera.transform.forward * _additionalPosition.z;

        newRot = _camera.transform.rotation * _additionalRotation;

        transform.position = newPos;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 12);
    }
}
