using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeData
{
    public string _BiomeName;
    public float _caveSize;
    public float _stalagmiteAndStalactiteFrequency;
    public float _startDepth;
    public List<Material> _stoneMaterials;
    public List<Minerals> _materials;
}

[System.Serializable]
public class PropData
{
    public GameObject _props;
    public Minerals _mineral;
}

[CreateAssetMenu(fileName = "Data", menuName = "Config/Generation Config", order = 1)]
public class GenerationConfig : ScriptableObject
{
    private float _currentCaveSize;
    private float _currentStalagmiteFrequency;
    private float _currentStalactiteFrequency;

    public BiomeData[] _biomes;
    public PropData[] _props;

}
public enum BiomeName
{
    UpperShafts = 0,
    MiddleShafts =  1,
    DeepMines = 2,
    MagmaDepths = 3,
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
    Glowing_mushroom, //Для чипов
}