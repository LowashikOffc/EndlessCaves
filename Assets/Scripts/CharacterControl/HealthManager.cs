using System;
using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    private bool _isDead;
    private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _healthRegen;
    [SerializeField] private float _healthRegentime;

    public event Action<float> Add;
    public event Action Death;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(Regeneration());
    }

    private void HealthAdd(float health)
    {
        _health += health;
        Add?.Invoke(_health);
        if (_health <= 0)
        {
            _isDead = true;
            Death?.Invoke();
        }
    }

    IEnumerator Regeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(_healthRegentime);
            if (_health >= _maxHealth || _isDead) continue;
            HealthAdd(_healthRegen);
        }
    }
}
