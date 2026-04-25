using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public GameObject _room;
    public Transform _start;
    public float _propScale;
    public List<Transform> _ends;
    public bool isBlocked; // ‘лаг блокировки спавна комнаты
}

public class CaveGenerating : MonoBehaviour
{
    [SerializeField] private List<Room> _rooms;
    [SerializeField] private GenerationConfig _generationConfig;
    [SerializeField] private GameObject _folder;
    [SerializeField] private GameObject _stonePrefab;
    [SerializeField] private GameObject _stalagmitePrefab;
    [SerializeField] private GameObject _stalactitePrefab;
    [SerializeField] private float _maxRooms;
    [SerializeField] private int _branchRoomID;
    [SerializeField] private int _branchGenerations = 3;
    [SerializeField] private bool _randomizeExitChoice = true;

    private Quaternion _lastExitRotation;
    private Vector3 _lastExitPosition;
    private int _mainGeneratedCount = 0;

    private void Start()
    {
        // »нициализаци€ комнат
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

        // √енераци€ основной линии
        for (int i = 0; i < _maxRooms; i++)
        {
            GenerateMain();
        }
    }

    private GameObject RoomSelect(int random, RoomTag tag)
    {
        // —оздаем список доступных (не заблокированных) комнат
        List<Room> availableRooms = new List<Room>();
        foreach (Room room in _rooms)
        {
            if (!room.isBlocked)
                availableRooms.Add(room);
        }

        if (availableRooms.Count == 0)
        {
            Debug.LogError("Ќет доступных комнат дл€ спавна! ¬се комнаты заблокированы.");
            return null;
        }

        // јдаптируем random индекс под доступные комнаты
        int adjustedRandom = random % availableRooms.Count;

        if (tag == RoomTag.branch && adjustedRandom == _branchRoomID % availableRooms.Count)
        {
            adjustedRandom = (adjustedRandom + 1) % availableRooms.Count;
        }

        GameObject newRoom = Instantiate(availableRooms[adjustedRandom]._room);
        newRoom.transform.SetParent(_folder.transform);
        return newRoom;
    }

    // ƒополнительный метод дл€ проверки, заблокирована ли комната по индексу
    private bool IsRoomBlocked(int roomIndex)
    {
        if (roomIndex >= 0 && roomIndex < _rooms.Count)
            return _rooms[roomIndex].isBlocked;
        return false;
    }

    // ћетод дл€ получени€ случайной незаблокированной комнаты
    private int GetRandomUnblockedRoomIndex()
    {
        List<int> unblockedIndices = new List<int>();
        for (int i = 0; i < _rooms.Count; i++)
        {
            if (!_rooms[i].isBlocked)
                unblockedIndices.Add(i);
        }

        if (unblockedIndices.Count == 0)
            return -1;

        return unblockedIndices[Random.Range(0, unblockedIndices.Count)];
    }

    private void GenerateStone(Vector3 position)
    {
        //GameObject stone = Instantiate(_stonePrefab);
        //stone.transform.SetParent(_folder.transform);
        //stone.transform.position = _lastExitPosition;
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
        int rand = GetRandomUnblockedRoomIndex();
        if (rand == -1)
        {
            Debug.LogError("Ќет доступных комнат дл€ генерации основной линии!");
            return;
        }

        GameObject newRoom = RoomSelect(rand, RoomTag.main);
        if (newRoom == null) return;

        Transform newStart = newRoom.transform.Find("StartPoint");
        List<Transform> ends = EndPointsFind(newRoom);

        if (ends.Count == 0)
        {
            Debug.LogError($" омната {newRoom.name} не имеет выходов!");
            Destroy(newRoom);
            return;
        }

        int mainExitIndex = _randomizeExitChoice ? Random.Range(0, ends.Count) : 0;
        Transform selectedEnd = ends[mainExitIndex];

        GenerateLogic(newRoom, newStart, selectedEnd);
        Visuals(newRoom, _rooms[rand]._propScale);

        _mainGeneratedCount++;

        if (ends.Count >= 2)
        {
            if (Random.Range(0, 2) == 1)
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
        int rand = GetRandomUnblockedRoomIndex();
        if (rand == -1)
        {
            Debug.LogWarning("Ќет доступных комнат дл€ генерации ветки!");
            return;
        }

        GameObject newRoom = RoomSelect(rand, RoomTag.branch);
        if (newRoom == null) return;
        Transform newStart = newRoom.transform.Find("StartPoint");
        List<Transform> ends = EndPointsFind(newRoom);

        if (ends.Count == 0)
        {
            Debug.LogError($"¬етка: комната {newRoom.name} не имеет выходов!");
            Destroy(newRoom);
            return;
        }

        int branchExitIndex = _randomizeExitChoice ? Random.Range(0, ends.Count) : 0;
        Transform newEnd = ends[branchExitIndex];

        GenerateLogic(newRoom, newStart, newEnd);
        Visuals(newRoom, _rooms[rand]._propScale);
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

    private void Visuals(GameObject room, float currentPropScaleMultiply)
    {
        foreach (Transform prop in room.transform)
        {
            if (prop.name.Contains("Stone"))
            {
                BiomeName biome = BiomeName.UpperShafts;
                MeshRenderer renderer = prop.GetComponent<MeshRenderer>();
                Vector3 pos = room.transform.position + prop.position;
                Vector2 scale = renderer.material.mainTextureScale;

                renderer.material.mainTextureScale = ScaleFormula(prop.transform.localScale, renderer.material.mainTextureScale, 0.3f);
                renderer.material = MaterialSelect(pos, biome);
                return;
            }
            prop.GetComponent<MeshRenderer>().enabled = false;
            if (prop.name.Contains("Stalagmite"))
            {
                if (Random.Range(0, 2) == 1)
                {
                    prop.GetComponent<MeshRenderer>().enabled = true;
                    GameObject newProp = Instantiate(_stalagmitePrefab);
                    newProp.transform.position = prop.transform.position;
                    newProp.transform.SetParent(prop.transform.parent);

                    BiomeName biome = BiomeName.UpperShafts;
                    MeshRenderer renderer = newProp.GetComponent<MeshRenderer>();
                    Vector3 pos = room.transform.position + prop.position;
                    Vector2 scale = renderer.material.mainTextureScale;
                    float roomMultiply = currentPropScaleMultiply;

                    newProp.transform.localScale = PropScaler(newProp.transform, roomMultiply, 1, 3);
                    renderer.material.mainTextureScale = ScaleFormula(newProp.transform.localScale, renderer.material.mainTextureScale, 0.1f);
                    renderer.material = MaterialSelect(pos, biome);
                }
            }
            if (prop.name.Contains("Stalactite"))
            {
                if (Random.Range(0, 2) == 1)
                {
                    prop.GetComponent<MeshRenderer>().enabled = true;
                    GameObject newProp = Instantiate(_stalactitePrefab);
                    newProp.transform.position = prop.transform.position;
                    newProp.transform.SetParent(prop.transform.parent);

                    BiomeName biome = BiomeName.UpperShafts;
                    MeshRenderer renderer = newProp.GetComponent<MeshRenderer>();
                    Vector3 pos = room.transform.position + prop.position;
                    Vector2 scale = renderer.material.mainTextureScale;
                    float roomMultiply = currentPropScaleMultiply;

                    newProp.transform.localScale = PropScaler(newProp.transform, roomMultiply, 1, 3);
                    renderer.material.mainTextureScale = ScaleFormula(newProp.transform.localScale, renderer.material.mainTextureScale, 0.1f);
                    renderer.material = MaterialSelect(pos, biome);
                }
            }
        }
    }

    private Material MaterialSelect(Vector3 pos, BiomeName biome)
    {
        if (pos.y <= _generationConfig._biomes[0]._startDepth + Random.Range(-100, 0)) biome = BiomeName.UpperShafts;
        if (pos.y <= _generationConfig._biomes[1]._startDepth + Random.Range(-100, 100)) biome = BiomeName.MiddleShafts;
        if (pos.y <= _generationConfig._biomes[2]._startDepth + Random.Range(-100, 100)) biome = BiomeName.DeepMines;
        if (pos.y <= _generationConfig._biomes[3]._startDepth + Random.Range(-100, 100)) biome = BiomeName.MagmaDepths;

        if (biome == BiomeName.UpperShafts)
        {
            foreach (Material m in _generationConfig._biomes[0]._stoneMaterials)
            {
                return m;
            }
        }
        else if (biome == BiomeName.MiddleShafts)
        {
            foreach (Material m in _generationConfig._biomes[1]._stoneMaterials)
            {
                return m;
            }
        }
        else if (biome == BiomeName.DeepMines)
        {
            foreach (Material m in _generationConfig._biomes[2]._stoneMaterials)
            {
                return m;
            }
        }
        else if (biome == BiomeName.MagmaDepths)
        {
            foreach (Material m in _generationConfig._biomes[3]._stoneMaterials)
            {
                return m;
            }
        }
        return null;
    }

    private Vector2 ScaleFormula(Vector2 startScale, Vector2 materialScale, float scaleMultiply)
    {
        Vector2 scale = new Vector2(startScale.x / scaleMultiply,
                       startScale.y / scaleMultiply);
        return scale;
    }

    private Vector3 PropScaler(Transform prop, float roomMultiply, float min, float max)
    {
        Vector3 newScale = prop.localScale * Random.Range(roomMultiply - min * roomMultiply, max * roomMultiply);
        return newScale;
    }
}

public enum RoomTag
{
    main = 1,
    branch = 2
}