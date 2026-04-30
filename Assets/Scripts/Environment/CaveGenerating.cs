using System.Collections.Generic;
using UnityEngine;

public class CaveGenerating : MonoBehaviour
{
    [SerializeField] private List<RoomMetadata> _roomPrefabs;
    [SerializeField] private GenerationConfig _generationConfig;
    [SerializeField] private GenerationRules _generationRules;
    [SerializeField] private GameObject _folder;
    [SerializeField] private GameObject _stalagmitePrefab;
    [SerializeField] private Transform _player;
    [SerializeField] private int _branchGenerations = 3;
    [SerializeField] private bool _randomizeExitChoice = true;
    [SerializeField] private float _subBranchChance = 0.3f;
    [SerializeField] private float _stalagmiteSpawnChance = 0.5f;
    [SerializeField] private float _biomeBoundaryJitter = 100f;

    private Quaternion _lastExitRotation;
    private Vector3 _lastExitPosition;
    private Vector3 _lastFailedFrontier;
    private bool _streamingStalled;

    private RoomSelector _selector;
    private RoomPlacer _placer;

    private void Start()
    {
        if (_generationRules == null)
        {
            Debug.LogError("CaveGenerating: GenerationRules not assigned");
            return;
        }
        if (_folder == null) _folder = gameObject;

        _selector = new RoomSelector(_roomPrefabs, _generationRules);
        _placer = new RoomPlacer(_folder.transform, _generationRules.MaxOverlapRetries);

        if (!SpawnStartZone()) return;

        for (int i = 0; i < _generationRules.StreamAheadCount; i++)
        {
            GenerateMain();
        }
    }

    private void Update()
    {
        if (_player == null || _generationRules == null) return;
        if (Vector3.Distance(_player.position, _lastExitPosition) >= _generationRules.StreamTriggerDistance) return;
        if (_streamingStalled && _lastExitPosition == _lastFailedFrontier) return;
        GenerateMain();
    }

    private bool SpawnStartZone()
    {
        if (_generationRules.StartZonePrefab == null)
        {
            Debug.LogError("CaveGenerating: StartZonePrefab not set in GenerationRules");
            return false;
        }
        GameObject startZone = Instantiate(_generationRules.StartZonePrefab, _folder.transform);
        RoomMetadata meta = startZone.GetComponent<RoomMetadata>();
        if (meta == null)
        {
            Debug.LogError("CaveGenerating: StartZonePrefab missing RoomMetadata component");
            Destroy(startZone);
            return false;
        }
        List<Transform> ends = meta.GetEnds();
        if (ends.Count == 0)
        {
            Debug.LogError("CaveGenerating: StartZone has no EndPoints");
            return false;
        }
        Transform exit = ends[0];
        _lastExitPosition = exit.position;
        _lastExitRotation = exit.rotation;

        _placer.RegisterPlaced(meta.Bounds);
        _selector.OnRoomPlaced(ResolveBiome(_lastExitPosition.y), RoomType.StartZone);
        Visuals(startZone);
        return true;
    }

    private void GenerateMain()
    {
        BiomeName biome = ResolveBiome(_lastExitPosition.y);
        RoomMetadata prefab = _selector.Pick(_lastExitPosition.y, biome);
        if (prefab == null)
        {
            StallStreaming($"no candidate room for biome {biome} at depth {_lastExitPosition.y}");
            return;
        }

        PlaceResult result = _placer.TryPlace(prefab, _lastExitPosition, _lastExitRotation, _randomizeExitChoice);
        if (!result.Success)
        {
            StallStreaming($"failed to place {prefab.name} after retries");
            return;
        }

        _streamingStalled = false;
        _selector.OnRoomPlaced(biome, prefab.Type);
        Visuals(result.Instance);

        Vector3 mainExitPos = result.PickedExit.position;
        Quaternion mainExitRot = result.PickedExit.rotation;

        if (result.RemainingExits.Count > 0)
        {
            ShuffleInPlace(result.RemainingExits);
            int branchesToMake = Mathf.Min(result.RemainingExits.Count, _branchGenerations);
            for (int i = 0; i < branchesToMake; i++)
            {
                GenerateBranchFromExit(result.RemainingExits[i]);
            }
        }

        _lastExitPosition = mainExitPos;
        _lastExitRotation = mainExitRot;
    }

    private void GenerateBranchFromExit(Transform exitPoint)
    {
        Vector3 saved = _lastExitPosition;
        Quaternion savedRot = _lastExitRotation;

        _lastExitPosition = exitPoint.position;
        _lastExitRotation = exitPoint.rotation;

        for (int i = 0; i < _branchGenerations; i++)
        {
            GenerateBranch();
        }

        _lastExitPosition = saved;
        _lastExitRotation = savedRot;
    }

    private void GenerateBranch()
    {
        BiomeName biome = ResolveBiome(_lastExitPosition.y);
        RoomMetadata prefab = _selector.Pick(_lastExitPosition.y, biome);
        if (prefab == null) return;

        PlaceResult result = _placer.TryPlace(prefab, _lastExitPosition, _lastExitRotation, _randomizeExitChoice);
        if (!result.Success) return;

        _selector.OnRoomPlaced(biome, prefab.Type);
        Visuals(result.Instance);

        _lastExitPosition = result.PickedExit.position;
        _lastExitRotation = result.PickedExit.rotation;

        if (result.RemainingExits.Count > 0 && _randomizeExitChoice && Random.value < _subBranchChance)
        {
            Vector3 saved = _lastExitPosition;
            Quaternion savedRot = _lastExitRotation;

            int idx = Random.Range(0, result.RemainingExits.Count);
            _lastExitPosition = result.RemainingExits[idx].position;
            _lastExitRotation = result.RemainingExits[idx].rotation;

            int subDepth = Mathf.Max(1, _branchGenerations / 2);
            for (int i = 0; i < subDepth; i++)
            {
                GenerateBranch();
            }

            _lastExitPosition = saved;
            _lastExitRotation = savedRot;
        }
    }

    private static void ShuffleInPlace(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void StallStreaming(string reason)
    {
        if (!_streamingStalled || _lastExitPosition != _lastFailedFrontier)
            Debug.LogWarning($"CaveGenerating: {reason}");
        _streamingStalled = true;
        _lastFailedFrontier = _lastExitPosition;
    }

    private BiomeName ResolveBiome(float y, float jitter = 0f)
    {
        BiomeName biome = BiomeName.UpperShafts;
        var biomes = _generationConfig != null ? _generationConfig._biomes : null;
        if (biomes == null) return biome;
        float Threshold(int i) => biomes[i]._startDepth + (jitter > 0f ? Random.Range(-jitter, jitter) : 0f);
        if (biomes.Length > 1 && y <= Threshold(1)) biome = BiomeName.MiddleShafts;
        if (biomes.Length > 2 && y <= Threshold(2)) biome = BiomeName.DeepMines;
        if (biomes.Length > 3 && y <= Threshold(3)) biome = BiomeName.MagmaDepths;
        return biome;
    }

    private void Visuals(GameObject room)
    {
        foreach (Transform prop in room.transform)
        {
            if (prop.name.Contains("Stone"))
            {
                ApplyBiomeMaterial(prop.position, prop.GetComponent<MeshRenderer>());
            }
            if (prop.name.Contains("Stalagmite") && Random.value < _stalagmiteSpawnChance && _stalagmitePrefab != null)
            {
                GameObject newProp = Instantiate(_stalagmitePrefab);
                newProp.transform.position = prop.position;
                newProp.transform.SetParent(prop);
                ApplyBiomeMaterial(prop.position, newProp.GetComponent<MeshRenderer>());
            }
        }
    }

    private void ApplyBiomeMaterial(Vector3 worldPos, MeshRenderer renderer)
    {
        if (renderer == null || _generationConfig == null) return;
        var biomes = _generationConfig._biomes;
        if (biomes == null) return;

        BiomeName biome = ResolveBiome(worldPos.y, _biomeBoundaryJitter);
        int idx = (int)biome;
        if (idx <= 0 || idx >= biomes.Length) return;
        var mats = biomes[idx]._stoneMaterials;
        if (mats == null || mats.Count == 0) return;
        renderer.material = mats[mats.Count - 1];
    }
}
