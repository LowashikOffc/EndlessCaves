using System;
using UnityEngine;



[Serializable]
class BiomeData
{
    [SerializeField] private float _caveSize;
    [SerializeField] private float _stalagmiteFrequency;
    [SerializeField] private float _stalactiteFrequency;
    [SerializeField] private Material _stoneMaterial;
    [SerializeField] private Materials _material;
}

[CreateAssetMenu(fileName = "Data", menuName = "Config/Generation Config", order = 1)]
public class GenerationConfig : ScriptableObject
{
    [SerializeField] private float _currentCaveSize;
    [SerializeField] private float _currentStalagmiteFrequency;
    [SerializeField] private float _currentStalactiteFrequency;

    [SerializeField] private BiomeData[] _biomes;
}

public enum Materials
{
    Iron,
    Gold
}