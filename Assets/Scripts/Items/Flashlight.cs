using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light _lightSource;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Vector3 _offset;
    private Camera _camera;
    private float _energy = 1000;
    private bool _canEnable = true;

    void Start()
    {
        InputReceiver.Instance.Flashlight += StateChange;
        _camera = Camera.main;
        StartCoroutine(FlEnergyDown());
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
        transform.position = Vector3.Lerp(transform.position, _camera.transform.position
            + _camera.transform.right * _offset.x
            + _camera.transform.up * _offset.y
            + _camera.transform.forward * _offset.z,
            Time.deltaTime * 10);

        transform.rotation = Quaternion.Lerp(transform.rotation, _camera.transform.rotation, Time.deltaTime * 10);

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
            yield return new WaitForSeconds(0.3f);
        }    
    }

    void Off()
    {
        _lightSource.enabled = false;
        SoundService.Instance.PlaySound3D(SoundID.flashlight, transform.position, 0.2f);
    }
}
