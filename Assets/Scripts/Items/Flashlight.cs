using System.Collections;
using TMPro;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light _lightSource;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Vector3 _offset;
    [Tooltip("Жёсткость 'руки' для позиции фонаря. " +
             "5–8 — приятный sway, 15+ — почти моментально.")]
    [SerializeField] private float _followSmoothing = 8f;

    [Tooltip("Жёсткость поворота фонаря в сторону прицела. " +
             "Можно сделать выше positional, чтобы луч точнее целился, при этом рука всё ещё качается.")]
    [SerializeField] private float _rotationSmoothing = 8f;
    private Camera _camera;
    private float _energy = 10000;
    private bool _canEnable = true;
    private Vector3 _lookPoint;

    void Start()
    {
        _camera = Camera.main;
        StartCoroutine(FlEnergyDown());

        if (InputReceiver.Instance == null) return;
        InputReceiver.Instance.Flashlight += StateChange;
        InputReceiver.Instance.CameraLookAngle += LookPoint;
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance == null) return;
        InputReceiver.Instance.Flashlight -= StateChange;
    }

    private void LookPoint(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            _lookPoint = hit.point;
        else
            _lookPoint = ray.origin + ray.direction * 100f;
    }

    private void StateChange()
    {
        if (!_lightSource.enabled && _energy > 0)
        {
            if (_canEnable == true)
            {
                _lightSource.enabled = true;
                SoundService.Instance.PlaySound3D(SoundID.flashlight, transform.position, 0.2f);
            }
        }
        else if (_lightSource.enabled)
        {
            _lightSource.enabled = false;
            SoundService.Instance.PlaySound3D(SoundID.flashlight, transform.position, 0.2f);
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = _camera.transform.position
            + _camera.transform.right * _offset.x
            + _camera.transform.up * _offset.y
            + _camera.transform.forward * _offset.z;

        float positionT = 1f - Mathf.Exp(-_followSmoothing * Time.deltaTime);
        float rotationT = 1f - Mathf.Exp(-_rotationSmoothing * Time.deltaTime);

        //transform.position = Vector3.Lerp(transform.position, targetPosition, positionT);

        //Vector3 aimAt = _lookPoint != Vector3.zero
        //    ? _lookPoint
        //    : _camera.transform.position + _camera.transform.forward * 100f;
        //Vector3 direction = aimAt - targetPosition;
        //if (direction.sqrMagnitude > 0.0001f)
        //{
        //    Quaternion desired = Quaternion.LookRotation(direction);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, desired, rotationT);
        //}

        transform.rotation = Quaternion.Slerp(transform.rotation, _camera.transform.rotation, rotationT);
        transform.position = Vector3.Slerp(transform.position, _camera.transform.position, positionT);

        _text.text = "Fl: " + _energy / 10;
    }
    IEnumerator FlEnergyDown()
    {
        while (true)
        {
            if (_lightSource.enabled == true && _energy > 0)
            {
                _energy -= 1f;
            }
            else if (_lightSource.enabled == true && _energy == 0)
            {
                if (_canEnable == true)
                {
                    _canEnable = false;
                    Off();
                }
            }
            yield return new WaitForSeconds(0.4f);
        }    
    }

    void Off()
    {
        _lightSource.enabled = false;
        SoundService.Instance.PlaySound3D(SoundID.flashlight, transform.position, 0.2f);
    }
}
