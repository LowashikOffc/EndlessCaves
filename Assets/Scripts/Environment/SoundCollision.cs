using UnityEngine;
using static Unity.VisualScripting.Member;

[System.Serializable]
public class Sources
{
    public AudioSource _audio;
    public AudioLowPassFilter _lpFilter;
    public float _volume;
    public float _filter;
}

public class SoundCollision : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Sources[] _sources;

    private float _currentVolume;
    public float _currentLowPass;

    private RaycastHit _hit;
    private void Start()
    {
        _camera = Camera.main;
        foreach (Sources source in _sources)
        {
            if (!source._audio.gameObject.GetComponent<AudioLowPassFilter>()) source._lpFilter = source._audio.gameObject.AddComponent<AudioLowPassFilter>();

            source._lpFilter.cutoffFrequency = 22000;
            source._volume = source._audio.volume;
            source._filter = source._lpFilter.cutoffFrequency;
            
        }
    }
    private void FixedUpdate()
    {
        Vector3 distance = _camera.transform.position - transform.position;
        Vector3 direction = distance.normalized;
        //Debug.Log(distance.magnitude);
        //Debug.DrawRay(transform.position, direction, Color.red, 0.5f);
        foreach (Sources source in _sources)
        {
            if (Physics.SphereCast(transform.position, 0.05f, direction, out _hit, 1000))
            {
                if (_hit.transform.gameObject && _hit.transform.tag != "Player")
                {
                    _currentVolume = source._volume / Mathf.Clamp(distance.magnitude, 1, Mathf.Infinity);
                    _currentLowPass = source._filter / Mathf.Clamp(distance.magnitude * 0.7f, 1, Mathf.Infinity);
                }
                else
                {
                    _currentVolume = source._volume;
                    _currentLowPass = source._filter;
                }
            }
            else
            {
                _currentVolume = source._volume;
                _currentLowPass = source._filter;
            }
            source._lpFilter.cutoffFrequency = Mathf.Lerp(source._lpFilter.cutoffFrequency, _currentLowPass, 0.04f);
            source._audio.volume = Mathf.Clamp(Mathf.Lerp(source._audio.volume, _currentVolume, 0.04f), 0.02f, 1);
            if (_currentVolume <= 0.01f) source._audio.volume = 0;
        }
    }
}
