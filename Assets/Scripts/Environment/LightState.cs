using UnityEngine;

public class LightState : MonoBehaviour
{
    [SerializeField] private Light[] _lights;

    [SerializeField] private bool _enabled;

    private void Start()
    {
        foreach (Light light in _lights)
        {
            light.gameObject.SetActive(_enabled);
        }
    }
    public void UpdateLight(bool enabled)
    {
        _enabled = enabled;
        foreach (Light light in _lights)
        {
            light.gameObject.SetActive(_enabled);
        }
    }
}
