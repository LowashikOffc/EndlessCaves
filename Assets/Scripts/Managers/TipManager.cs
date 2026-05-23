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

    private List<Tips> _currentTips;
    public event Action<Tips[]> _update;

    public void AddTip(Tips tip)
    {
        _currentTips.Add(tip);
    }
    public void DeleteTip(Tips tip)
    {
        foreach (var t in _currentTips)
        {
            if (t == tip)
            {
                _currentTips.Remove(t);
            }
        }
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