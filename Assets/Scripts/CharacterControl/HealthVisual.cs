using UnityEngine;

public class HealthVisual : MonoBehaviour
{
    [SerializeField] private HealthManager _healthManager;
    private float _currentHealth;
    private void Start()
    {
        _healthManager.Add += Visuals;
    }

    private void OnDestroy()
    {
        _healthManager.Add -= Visuals;
    }
    private void Visuals(float health)
    {
        _currentHealth = health;
        Debug.Log($"Current health: {_currentHealth}");
    }
}
