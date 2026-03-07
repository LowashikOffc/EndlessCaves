using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _health;

    public event Action<int> Add;
    public event Action Death;

    private void HealthAdd(int health)
    {
        _health += health;
        Add?.Invoke(_health);
        if (_health <= 0) Death?.Invoke();
    }
}
