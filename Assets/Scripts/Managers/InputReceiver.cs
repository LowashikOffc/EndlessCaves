using System;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    public static InputReceiver Instance { get; private set; }

    public event Action<bool> _w;
    public event Action<bool> _a;
    public event Action<bool> _s;
    public event Action<bool> _d;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) _w?.Invoke(true);
        else _w?.Invoke(false);

        if (Input.GetKey(KeyCode.A)) _a?.Invoke(true);
        else _a?.Invoke(false);

        if (Input.GetKey(KeyCode.S)) _s?.Invoke(true);
        else _s?.Invoke(false);

        if (Input.GetKey(KeyCode.D)) _d?.Invoke(true);
        else _d?.Invoke(false);
    }
}
