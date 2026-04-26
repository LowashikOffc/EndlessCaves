using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomTypeQuota
{
    public RoomType Type;
    public int MinCount;
    public float Weight = 1f;
}

[System.Serializable]
public class BiomeRules
{
    public BiomeName Biome;
    public List<RoomTypeQuota> Quotas;
}

[CreateAssetMenu(fileName = "GenerationRules", menuName = "Config/Generation Rules", order = 2)]
public class GenerationRules : ScriptableObject
{
    public List<BiomeRules> BiomeRulesList;
    public GameObject StartZonePrefab;
    public int StreamAheadCount = 5;
    public int MaxOverlapRetries = 3;
    public float StreamTriggerDistance = 50f;

    public BiomeRules GetRulesForBiome(BiomeName biome)
    {
        if (BiomeRulesList == null) return null;
        foreach (var br in BiomeRulesList)
            if (br.Biome == biome) return br;
        return null;
    }
}
