using System;
using UnityEngine;



[Serializable]
class BiomeData
{
    [SerializeField] private float _caveSize;
    [SerializeField] private float _stalagmiteFrequency;
    [SerializeField] private float _stalactiteFrequency;
    [SerializeField] private Material _stoneMaterial;
    [SerializeField] private Minerals _material;
}

[CreateAssetMenu(fileName = "Data", menuName = "Config/Generation Config", order = 1)]
public class GenerationConfig : ScriptableObject
{
    private float _currentCaveSize;
    private float _currentStalagmiteFrequency;
    private float _currentStalactiteFrequency;

    [SerializeField] private BiomeData[] _biomes;
}

public enum Minerals
{
    Iron,
    Gold,
    Copper,
    Coal,
    Amethyst,
    Quartz,
    Torbernite, //Радиоактивный
    Silicon, //Для чипов

}