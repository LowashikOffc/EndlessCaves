using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Room
{
    public GameObject _room;
    public Transform _start;
    public List<Transform> _ends;
}

public class CaveGenerating : MonoBehaviour
{
    [SerializeField] private List<Room> _rooms;
    [SerializeField] private GenerationConfig _generationConfig;
    [SerializeField] private GameObject _folder;
    [SerializeField] private GameObject _stonePrefab;
    [SerializeField] private float _maxRooms;
    [SerializeField] private int _branchRoomID;
    [SerializeField] private int _branchGenerations = 3;
    [SerializeField] private bool _randomizeExitChoice = true;

    private Quaternion _lastExitRotation;
    private Vector3 _lastExitPosition;
    private int _mainGeneratedCount = 0;

    private void Start()
    {
        // Инициализация комнат
        foreach (Room room in _rooms)
        {
            Transform currentRoom = room._room.transform;
            room._ends.Clear();

            foreach (Transform g in currentRoom)
            {
                if (g.name == "StartPoint")
                    room._start = g;
                else if (g.name.Contains("EndPoint"))
                    room._ends.Add(g);
            }
        }

        // Генерация основной линии
        for (int i = 0; i < _maxRooms; i++)
        {
            GenerateMain();
        }
    }

    private GameObject RoomSelect(int random, RoomTag tag)
    {
        if (tag == RoomTag.branch && random == _branchRoomID)
        {
             random = (random + 1) % _rooms.Count;
        }

        GameObject newRoom = Instantiate(_rooms[random]._room);
        newRoom.transform.SetParent(_folder.transform);
        return newRoom;
    }

    private void GenerateStone(Vector3 position)
    {
        GameObject stone = Instantiate(_stonePrefab);
        stone.transform.SetParent(_folder.transform);
        stone.transform.position = _lastExitPosition;
    }

    private List<Transform> EndPointsFind(GameObject room)
    {
        List<Transform> ends = new List<Transform>();
        Transform roomTransform = room.transform;

        foreach (Transform child in roomTransform)
        {
            if (child.name.Contains("EndPoint"))
            {
                ends.Add(child);
            }
        }

        return ends;
    }

    private void GenerateLogic(GameObject room, Transform start, Transform end)
    {
        Quaternion rotationOffset = room.transform.rotation * Quaternion.Inverse(start.rotation);
        room.transform.rotation = _lastExitRotation * rotationOffset;

        Vector3 positionOffset = room.transform.position - start.position;
        room.transform.position = _lastExitPosition + positionOffset;

        _lastExitPosition = end.position;
        _lastExitRotation = end.rotation;
    }

    public void GenerateMain()
    {
        int rand = Random.Range(0, _rooms.Count);
        GameObject newRoom = RoomSelect(rand, RoomTag.main);
        Transform newStart = newRoom.transform.Find("StartPoint");
        List<Transform> ends = EndPointsFind(newRoom);

        if (ends.Count == 0)
        {
            Debug.LogError($"Комната {newRoom.name} не имеет выходов!");
            Destroy(newRoom);
            return;
        }

        int mainExitIndex = _randomizeExitChoice ? Random.Range(0, ends.Count) : 0;
        Transform selectedEnd = ends[mainExitIndex];

        GenerateLogic(newRoom, newStart, selectedEnd);
        Visuals(newRoom);

        _mainGeneratedCount++;

        if (ends.Count >= 2)
        {
            Vector3 mainExitPosition = _lastExitPosition;
            Quaternion mainExitRotation = _lastExitRotation;

            List<int> branchExitIndices = new List<int>();
            for (int i = 0; i < ends.Count; i++)
            {
                if (i != mainExitIndex)
                    branchExitIndices.Add(i);
            }

            for (int i = 0; i < branchExitIndices.Count; i++)
            {
                int randomIndex = Random.Range(i, branchExitIndices.Count);
                int temp = branchExitIndices[i];
                branchExitIndices[i] = branchExitIndices[randomIndex];
                branchExitIndices[randomIndex] = temp;
            }

            int branchesToGenerate = Mathf.Min(branchExitIndices.Count, _branchGenerations);
            for (int i = 0; i < branchesToGenerate; i++)
            {
                int branchIndex = branchExitIndices[i];
                Transform branchEnd = ends[branchIndex];

                _lastExitPosition = selectedEnd.position;
                _lastExitRotation = selectedEnd.rotation;

                GenerateBranchFromExit(branchEnd);
            }

            _lastExitPosition = mainExitPosition;
            _lastExitRotation = mainExitRotation;
        }
    }

    private void GenerateBranchFromExit(Transform exitPoint)
    {
        Vector3 currentPosition = _lastExitPosition;
        Quaternion currentRotation = _lastExitRotation;

        _lastExitPosition = exitPoint.position;
        _lastExitRotation = exitPoint.rotation;

        if (Random.Range(0, 3) == 0)
        {
            GenerateStone(_lastExitPosition);
            //return;
        }

        for (int i = 0; i < _branchGenerations; i++)
        {
            GenerateBranch();
        }

        _lastExitPosition = currentPosition;
        _lastExitRotation = currentRotation;
    }

    public void GenerateBranch()
    {
        int rand = Random.Range(0, _rooms.Count);
        GameObject newRoom = RoomSelect(rand, RoomTag.branch);
        if (newRoom == null) return;
        Transform newStart = newRoom.transform.Find("StartPoint");
        List<Transform> ends = EndPointsFind(newRoom);

        if (ends.Count == 0)
        {
            Debug.LogError($"Ветка: комната {newRoom.name} не имеет выходов!");
            Destroy(newRoom);
            return;
        }

        int branchExitIndex = _randomizeExitChoice ? Random.Range(0, ends.Count) : 0;
        Transform newEnd = ends[branchExitIndex];

        GenerateLogic(newRoom, newStart, newEnd);
        Visuals(newRoom);
        if (ends.Count >= 2 && _randomizeExitChoice)
        {
            float subBranchChance = 0.3f;
            if (Random.value < subBranchChance)
            {
                List<int> otherExits = new List<int>();
                for (int i = 0; i < ends.Count; i++)
                {
                    if (i != branchExitIndex)
                        otherExits.Add(i);
                }

                if (otherExits.Count > 0)
                {
                    int subBranchIndex = otherExits[Random.Range(0, otherExits.Count)];
                    Transform subBranchEnd = ends[subBranchIndex];

                    Vector3 savedPos = _lastExitPosition;
                    Quaternion savedRot = _lastExitRotation;

                    _lastExitPosition = newEnd.position;
                    _lastExitRotation = newEnd.rotation;

                    for (int i = 0; i < _branchGenerations / 2; i++)
                    {
                        GenerateBranch();
                    }

                    _lastExitPosition = savedPos;
                    _lastExitRotation = savedRot;
                }
            }
        }
    }
    private void Visuals(GameObject room)
    {
        foreach (Transform stone in room.transform)
        {
            if (stone.name.Contains("Stone"))
            {
                BiomeName biome = BiomeName.UpperShafts;
                Vector3 pos = room.transform.position + stone.position;

                if (pos.y <= _generationConfig._biomes[1]._startDepth + Random.Range(-100, 100)) biome = BiomeName.MiddleShafts;
                if (pos.y <= _generationConfig._biomes[2]._startDepth + Random.Range(-100, 100)) biome = BiomeName.DeepMines;
                if (pos.y <= _generationConfig._biomes[3]._startDepth + Random.Range(-100, 100)) biome = BiomeName.MagmaDepths;

                Debug.Log(pos);
                MeshRenderer renderer = stone.GetComponent<MeshRenderer>();
                if (biome == BiomeName.MiddleShafts) 
                {
                    foreach (Material m in _generationConfig._biomes[1]._stoneMaterials)
                    {
                        renderer.material = m;
                    }
                }
                else if (biome == BiomeName.DeepMines)
                {
                    foreach (Material m in _generationConfig._biomes[2]._stoneMaterials)
                    {
                        renderer.material = m;
                    }
                }
                else if (biome == BiomeName.MagmaDepths)
                {
                    foreach (Material m in _generationConfig._biomes[3]._stoneMaterials)
                    {
                        renderer.material = m;
                    }
                }
            }
        }
    }
}

public enum RoomTag
{
    main = 1,
    branch = 2
}