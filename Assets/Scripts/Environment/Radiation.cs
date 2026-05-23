using System;
using UnityEngine;

public class Radiation : MonoBehaviour
{
    [SerializeField] private float _radiationAmount; // мкЗв/ч на максимальной дистанции
    [SerializeField] private float _radius = 5f;     // радиус поражения в метрах
    [SerializeField] private CrystalRandom _crystalRandom;

    private int _crystals;
    private float _currentRad = 0f;
    private GameObject _dosimeter;
    private Dosimeter _dosimeterScript;


    private void Start()
    {
        GameManager.Instance.GetRadiation(this);
        _crystalRandom._setCount += SetCount;
    }

    private void SetCount(int count)
    {
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

        // Проверяем, находится ли дозиметр внутри сферы радиации
        float dist = Vector3.Distance(transform.position, _dosimeter.transform.position);
        if (dist <= _radius)
        {

            // Линейное падение радиации с расстоянием
            // На границе _radius -> 0, в центре -> _radiationAmount
            float t = 1f - Mathf.Clamp01(dist / _radius);
            _currentRad = _radiationAmount * t * _crystals;

            //Debug.Log($"t: {t:F4}");
            //Debug.Log($"_radiationAmount: {_radiationAmount}");
            //Debug.Log($"_currentRad (power): {_currentRad:F2} μSv/h");
            //Debug.Log($"Time.fixedDeltaTime: {Time.fixedDeltaTime}");
            //Debug.Log($"Value sent to dosimeter: {_currentRad * Time.fixedDeltaTime}");

            // Передаём радиацию в дозиметр (мкЗв/ч)
            _dosimeterScript.AddRadiation(_currentRad * Time.fixedDeltaTime); // умножаем на время, если AddRadiation ожидает дозу, а не мощность
        }
        else
        {
            _currentRad = 0f;
        }
        //Debug.Log($"Base power: {_currentRad}");
    }

    // Опционально: визуализация в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, _radius);
    }
}