using System.Collections.Generic;
using UnityEngine;

public class PlaceResult
{
    public bool Success;
    public GameObject Instance;
    public RoomMetadata Metadata;
    public Transform PickedExit;
    public List<Transform> RemainingExits;
}

public class RoomPlacer
{
    private readonly Transform _parent;
    private readonly int _maxRetries;
    private readonly List<BoxCollider> _placedBounds = new List<BoxCollider>();

    public RoomPlacer(Transform parent, int maxRetries)
    {
        _parent = parent;
        _maxRetries = Mathf.Max(1, maxRetries);
    }

    public PlaceResult TryPlace(RoomMetadata prefab, Vector3 anchorPos, Quaternion anchorRot, bool randomizeExit)
    {
        for (int attempt = 0; attempt < _maxRetries; attempt++)
        {
            RoomMetadata meta = Object.Instantiate(prefab, _parent);
            GameObject instance = meta.gameObject;
            Transform start = meta.GetStart();
            List<Transform> ends = meta.GetEnds();

            if (start == null || ends.Count == 0)
            {
                Debug.LogError($"Room {prefab.name}: missing StartPoint or EndPoints");
                Object.Destroy(instance);
                return new PlaceResult { Success = false };
            }

            if (meta.Bounds == null)
            {
                Debug.LogError($"Room {prefab.name}: RoomMetadata.Bounds not assigned, cannot check overlap");
                Object.Destroy(instance);
                return new PlaceResult { Success = false };
            }

            Snap(instance.transform, start, anchorPos, anchorRot);
            Physics.SyncTransforms();

            if (Overlaps(meta.Bounds))
            {
                Object.Destroy(instance);
                continue;
            }

            int exitIndex = randomizeExit ? Random.Range(0, ends.Count) : 0;
            Transform picked = ends[exitIndex];
            List<Transform> remaining = new List<Transform>(ends.Count - 1);
            for (int i = 0; i < ends.Count; i++)
                if (i != exitIndex) remaining.Add(ends[i]);

            if (meta.Bounds != null)
                _placedBounds.Add(meta.Bounds);

            return new PlaceResult
            {
                Success = true,
                Instance = instance,
                Metadata = meta,
                PickedExit = picked,
                RemainingExits = remaining
            };
        }
        return new PlaceResult { Success = false };
    }

    public void RegisterPlaced(BoxCollider bounds)
    {
        if (bounds != null) _placedBounds.Add(bounds);
    }

    public void UnregisterBounds(BoxCollider bounds)
    {
        if (bounds != null) _placedBounds.Remove(bounds);
    }

    private static void Snap(Transform room, Transform start, Vector3 anchorPos, Quaternion anchorRot)
    {
        Quaternion rotationOffset = room.rotation * Quaternion.Inverse(start.rotation);
        room.rotation = anchorRot * rotationOffset;
        // После смены rotation дочерний start ещё не отражает новую матрицу — синхронизируем,
        // иначе positionOffset считается по устаревшим world-координатам и комнаты не стыкуются.
        Physics.SyncTransforms();
        Vector3 positionOffset = room.position - start.position;
        room.position = anchorPos + positionOffset;
    }

    private bool Overlaps(BoxCollider candidate)
    {
        if (candidate == null) return false;
        Bounds candidateBounds = candidate.bounds;
        candidateBounds.Expand(-0.1f);
        foreach (var existing in _placedBounds)
        {
            if (existing == null || existing == candidate) continue;
            if (candidateBounds.Intersects(existing.bounds))
                return true;
        }
        return false;
    }
}
