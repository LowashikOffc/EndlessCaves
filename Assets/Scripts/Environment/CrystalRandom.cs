using System;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRandom : MonoBehaviour
{
    [SerializeField] private List<GameObject> _crystals;
    [SerializeField] private int _activeCrystals;
    public event Action<int> _setCount;

    void Start()
    {
        if (_crystals.Count <= 0) return;
        Randomize();
    }

    private void ActivateAll()
    {
        _activeCrystals = _crystals.Count;
        foreach (var cryst in _crystals)
        {
            cryst.gameObject.SetActive(true);
        }
    }
    private void Randomize()
    {
        ActivateAll();
        foreach (var cryst in _crystals)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                cryst.gameObject.SetActive(false);
                _activeCrystals--; 
            }
        }
        if (_activeCrystals <= 0) Randomize();
        else
        {
            _setCount?.Invoke(_activeCrystals);
        }
    }

}
