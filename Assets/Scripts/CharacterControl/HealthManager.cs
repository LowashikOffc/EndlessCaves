using System;
using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _healthRegen;
    [SerializeField] private float _healthRegentime;

    public event Action<float> Add;
    public event Action Death;

    private void Start()
    {

    }

    private void HealthAdd(float health)
    {
        _health += health;
        Add?.Invoke(_health);
        if (_health <= 0) Death?.Invoke();
    }

    IEnumerator Regeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(_healthRegentime);
            HealthAdd(_healthRegen);
        }
    }
}
