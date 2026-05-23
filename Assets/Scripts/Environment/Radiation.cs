using System;
using UnityEngine;

public class Radiation : MonoBehaviour
{
    [SerializeField] private float _radiationAmount; // мкЗв/ч на максимальной дистанции
    [SerializeField] private float _radius = 5f;     // радиус поражения в метрах
    [SerializeField] private CrystalRandom _crystalRandom;

    [SerializeField] private int _crystals;
    [SerializeField] private float _currentRad = 0f;
    private GameObject _dosimeter;
    private Dosimeter _dosimeterScript;


    private void Awake()
    {
        GameManager.Instance.GetRadiation(this);
        _crystalRandom._setCount += SetCount;
    }
    private void OnDestroy()
    {
        _crystalRandom._setCount -= SetCount;
    }

    private void SetCount(int count)
    {
        Debug.Log(count);
        _crystals = count;
    }

    public void SetDosimeter(GameObject item)
    {
        _dosimeter = item;
        _dosimeterScript = _dosimeter.GetComponent<Dosimeter>();
    }

    public float GetRadiation()
    {
        return _currentRad;
    }

    private void FixedUpdate()
    {
        if (_dosimeter == null || _dosimeterScript == null)
        {
            _currentRad = 0f;
            return;
        }

        float dist = Vector3.Distance(transform.position, _dosimeter.transform.position);
        if (dist <= _radius)
        {

            float t = 1f - Mathf.Clamp01(dist / _radius);
            _currentRad = _radiationAmount * t * _crystals;

            _dosimeterScript.AddRadiation(_currentRad * Time.fixedDeltaTime);
        }
        else
        {
            _currentRad = 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, _radius);
    }
}