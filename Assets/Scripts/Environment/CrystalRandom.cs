using System;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRandom : MonoBehaviour
{
    [SerializeField] private List<GameObject> _crystals;
    [SerializeField] private int _activeCrystals;
    public event Action<int> _setCount;

    void Awake()
    {
        if (_crystals.Count <= 0) return;
        foreach (var cryst in _crystals)
        {
            if (UnityEngine.Random.Range(0, 2) == 0) cryst.gameObject.SetActive(false);
            else _activeCrystals++;
        }
        int a = 0;
        foreach (var cryst in _crystals)
        {
            if (!cryst.activeSelf)
            {
                a++;
            }
        }
        if (a == _crystals.Count) _crystals[UnityEngine.Random.Range(0,_crystals.Count)].SetActive(true);
        _setCount?.Invoke(a);
    }
}
