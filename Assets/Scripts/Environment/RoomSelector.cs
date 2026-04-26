using System.Collections.Generic;
using UnityEngine;

public class RoomSelector
{
    private readonly List<RoomMetadata> _prefabs;
    private readonly GenerationRules _rules;
    private readonly Dictionary<BiomeName, Dictionary<RoomType, int>> _placedCounts =
        new Dictionary<BiomeName, Dictionary<RoomType, int>>();

    public RoomSelector(List<RoomMetadata> prefabs, GenerationRules rules)
    {
        _prefabs = prefabs;
        _rules = rules;
    }

    public RoomMetadata Pick(float depthY, BiomeName biome)
    {
        BiomeRules rules = _rules != null ? _rules.GetRulesForBiome(biome) : null;
        List<RoomMetadata> candidates = new List<RoomMetadata>();
        foreach (var p in _prefabs)
        {
            if (p == null) continue;
            if (p.Type == RoomType.StartZone) continue;
            if (depthY < p.MinDepth || depthY > p.MaxDepth) continue;
            candidates.Add(p);
        }
        if (candidates.Count == 0) return null;

        if (rules != null && rules.Quotas != null)
        {
            List<RoomMetadata> deficit = new List<RoomMetadata>();
            foreach (var q in rules.Quotas)
            {
                if (GetPlacedCount(biome, q.Type) >= q.MinCount) continue;
                foreach (var c in candidates)
                    if (c.Type == q.Type) deficit.Add(c);
            }
            if (deficit.Count > 0)
                return WeightedPick(deficit, rules);
        }

        return WeightedPick(candidates, rules);
    }

    public void OnRoomPlaced(BiomeName biome, RoomType type)
    {
        if (!_placedCounts.TryGetValue(biome, out var dict))
        {
            dict = new Dictionary<RoomType, int>();
            _placedCounts[biome] = dict;
        }
        dict[type] = dict.TryGetValue(type, out int n) ? n + 1 : 1;
    }

    private RoomMetadata WeightedPick(List<RoomMetadata> candidates, BiomeRules rules)
    {
        float total = 0f;
        foreach (var c in candidates)
            total += GetEffectiveWeight(c, rules);

        if (total <= 0f)
            return candidates[Random.Range(0, candidates.Count)];

        float r = Random.value * total;
        float acc = 0f;
        foreach (var c in candidates)
        {
            acc += GetEffectiveWeight(c, rules);
            if (r <= acc) return c;
        }
        return candidates[candidates.Count - 1];
    }

    private float GetEffectiveWeight(RoomMetadata m, BiomeRules rules)
    {
        if (rules == null || rules.Quotas == null) return Mathf.Max(0f, m.Weight);
        foreach (var q in rules.Quotas)
            if (q.Type == m.Type) return Mathf.Max(0f, m.Weight * q.Weight);
        return Mathf.Max(0f, m.Weight);
    }

    private int GetPlacedCount(BiomeName biome, RoomType type)
    {
        if (!_placedCounts.TryGetValue(biome, out var dict)) return 0;
        return dict.TryGetValue(type, out int n) ? n : 0;
    }
}
