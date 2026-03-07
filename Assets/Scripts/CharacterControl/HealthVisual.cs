using UnityEngine;

public class HealthVisual : MonoBehaviour
{
    [SerializeField] private HealthManager _healthManager;
    private int _currentHealth;
    private void Start()
    {
        _healthManager.Add += Visuals;
    }

    private void Visuals(int health)
    {
        _currentHealth = health;
        Debug.Log($"Current health: {_currentHealth}");
    }
}
