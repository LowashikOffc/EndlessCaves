using System;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour
{
    public static TipManager Instance { get; private set; }
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

    public event Action<Tips, settingsEnum> Add;
    public event Action<Tips> Delete;

    public void AddTip(Tips tip, settingsEnum Enum)
    {
        Add?.Invoke(tip, Enum);
    }
    public void DeleteTip(Tips tip)
    {
        Delete?.Invoke(tip);
    }
}

public enum Tips
{
    Flashlight,
    Pickup,
    Drop,
    Scan,
    Hook_Throw,
    Hook_Return
}