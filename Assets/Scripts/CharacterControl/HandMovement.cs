using UnityEngine;

public class HandMovement : MonoBehaviour
{
    private Quaternion _objectAdditionalRotation;
    private Vector3 _objectAdditionalPosition;
    [SerializeField] private Vector3 _additionalPosition;
    [SerializeField] private Quaternion _additionalRotation;
    private Camera _camera;
    private Vector3 newPos = Vector3.zero;
    private Quaternion newRot = Quaternion.identity;
    private bool _smooth;
    private GameObject _currentObject;
    void Start()
    {
        _camera = Camera.main;
    }

    private void Awake()
    {
        InventoryManager.Instance.ChangeV3AndRot += ChangeAdditionalRotation;
    }

    private void ChangeAdditionalRotation(Vector3 pos, Quaternion rot, GameObject current)
    {
        _objectAdditionalRotation = rot;
        _objectAdditionalPosition = pos;

        if (_currentObject != current)
        {
            _smooth = false;
            transform.position = newPos;
            transform.rotation = newRot;
        }
        else
        {
            _smooth = true;
        }
        _currentObject = current;
    }

    private void CalculatePos()
    {
        newPos = _camera.transform.position
            + _camera.transform.right * (_additionalPosition.x + _objectAdditionalPosition.x)
            + _camera.transform.up * (_additionalPosition.y + _objectAdditionalPosition.y)
            + _camera.transform.forward * (_additionalPosition.z + _objectAdditionalPosition.z);

        newRot = _camera.transform.rotation * _additionalRotation * _objectAdditionalRotation;
    }

    private void SetPos()
    {
        transform.position = newPos;
        if (_smooth) transform.rotation = newRot;//Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 12);
        else transform.rotation = newRot;
    }

    void Update()
    {
        CalculatePos();
        SetPos();
    }
}
