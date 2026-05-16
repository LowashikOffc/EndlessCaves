using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Radiation : MonoBehaviour
{
    [SerializeField] private int _radiationAmount;
    float _currentRad = 0;
    private GameObject _player;
    private Collider _collider;
    private float _maxDist;
    private GameObject _dosimeter;
    private Dosimeter _dosimeterScript;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public float GetRadiation()
    {
        return _currentRad;
    }

    private void FixedUpdate()
    {
        if (!_dosimeter)
        {
            Debug.Log("dosimeter not found");
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject item in items)
            {
                if (item.GetComponent<Dosimeter>())
                {
                    _dosimeter = item;
                    _dosimeterScript = item.GetComponent<Dosimeter>();
                }
            }
        }
        else
        {
            float dist = Vector3.Distance(_dosimeter.transform.position.normalized, transform.position.normalized);
            float t = 1f - Mathf.Clamp01(dist / _maxDist);
            _currentRad = _radiationAmount * t;
            _dosimeterScript.AddRadiation(_currentRad);
        }
        //Debug.Log(_currentRad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_dosimeter) _maxDist = 0.5f;
        Debug.Log(_maxDist);
    }

    private void OnTriggerExit(Collider other)
    {
        _currentRad = 0;
    }

}
