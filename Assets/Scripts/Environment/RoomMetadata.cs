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
    [Header("Тип комнаты")]
    [Tooltip("Категория комнаты для подбора генератором. Например, NarrowPassage — узкий проход, LargeRoom — большой зал, " +
             "VerticalDrop — обрыв вниз. StartZone — спец-тип для стартовой зоны (на обычных комнатах НЕ ставить).")]
    public RoomType Type;

    [Header("Диапазон глубины Y, на которой комната может появиться")]
    [Tooltip("Минимальная глубина (worldY игрока), ниже которой комната не появляется. " +
             "В EndlessCaves Y уходит вниз, значит чем глубже — тем меньше Y. " +
             "Оставь по умолчанию (-Infinity), если ограничения снизу нет.")]
    public float MinDepth = float.NegativeInfinity;

    [Tooltip("Максимальная глубина (worldY игрока), выше которой комната уже не появляется. " +
             "Оставь по умолчанию (+Infinity), если ограничения сверху нет.")]
    public float MaxDepth = float.PositiveInfinity;

    [Header("Вес в выборе генератора")]
    [Tooltip("Вес комнаты в weighted-pick. 1 — обычная, >1 — чаще, <1 — реже, 0 — никогда. " +
             "Биом-правила (GenerationRules > Quotas) могут домножать этот вес на свой коэффициент.")]
    public float Weight = 1f;

    [Header("Границы комнаты (для проверки пересечений)")]
    [Tooltip("BoxCollider, описывающий габариты комнаты в мире. Используется для проверки overlap-а с другими комнатами. " +
             "Префаб обязан иметь BoxCollider — его удобно сделать триггером (Is Trigger = true), " +
             "чтобы он не мешал физике игрока. Если оставить пустым — компонент возьмёт BoxCollider с этого же объекта (Reset).")]
    public BoxCollider Bounds;

    private Transform _cachedStart;

    [Tooltip("Кэш дочерних EndPoint-ов. Заполняется автоматически при первом GetEnds(). " +
             "В инспекторе лучше оставить пустым.")]
    [SerializeField] private List<Transform> _cachedEnds;

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
        if (_cachedEnds == null) _cachedEnds = new List<Transform>();
        if (_cachedEnds.Count != 0) return _cachedEnds;
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
