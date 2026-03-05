using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class BiomeData
{
    [SerializeField] private string _BiomeName;
    [SerializeField] private float _caveSize;
    [SerializeField] private float _stalagmiteFrequency;
    [SerializeField] private float _stalactiteFrequency;
    [SerializeField] private List<Material> _stoneMaterials;
    [SerializeField] private List<Minerals> _materials;
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
    Silver,
    Gold,
    Lead,
    Copper,
    Amethyst,
    Quartz,
    Torbernite, //Радиоактивный
    Silicon, //Для чипов

}