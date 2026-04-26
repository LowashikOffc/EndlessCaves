using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    NarrowPassage,
    Maze,
    Debris,
    LargeRoom,
    River,
    VerticalDrop,
    StartZone
}

[RequireComponent(typeof(BoxCollider))]
public class RoomMetadata : MonoBehaviour
{
    public RoomType Type;
    public float MinDepth = float.NegativeInfinity;
    public float MaxDepth = float.PositiveInfinity;
    public float Weight = 1f;
    public BoxCollider Bounds;

    private Transform _cachedStart;
    private List<Transform> _cachedEnds;

    public Transform GetStart()
    {
        if (_cachedStart != null) return _cachedStart;
        foreach (Transform child in transform)
        {
            if (child.name == "StartPoint")
            {
                _cachedStart = child;
                break;
            }
        }
        return _cachedStart;
    }

    public List<Transform> GetEnds()
    {
        if (_cachedEnds != null) return _cachedEnds;
        _cachedEnds = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.name.Contains("EndPoint"))
                _cachedEnds.Add(child);
        }
        return _cachedEnds;
    }

    private void Reset()
    {
        Bounds = GetComponent<BoxCollider>();
    }
}
