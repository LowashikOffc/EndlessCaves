using UnityEngine;

[System.Serializable]
public class WireProp
{
    //public Material _material;
    public GameObject _wire;
    public float _width;
}
public class WireGeneration : MonoBehaviour
{
    [SerializeField] private Mesh _wireMesh;
    [SerializeField] private Mesh _connectorMesh;
    [SerializeField] private float _connectorWidth;
    [SerializeField] private float _wireWidth;
    [SerializeField] private bool _debugLine;
    [SerializeField] private Material _wireMaterial;
    [SerializeField] private Material _ConnectorMaterial;
    [SerializeField] private WireProp[] _wires;
    void Start()
    {
        if (_wires == null || _wires.Length < 2)
        {
            //Debug.LogError($"{_wires.Length} not enough. Need 2 or more");
            return;
        }
        //Debug.Log($"{_wires.Length} enough. Start creating wires");
        int current = 0;
        foreach (WireProp prop in _wires)
        {
            if (current > 0) CreateConnector(prop, current);
            CreateWire(prop, current);
            if (current < _wires.Length - 2) current++;
            else return;
        }
        CreateConnector(_wires[_wires.Length - 1], _wires.Length - 1);
        foreach (WireProp prop in _wires)
        {
            Destroy(prop._wire);
        }
    }

    private void CreateConnector(WireProp prop, int current)
    {
        //Debug.Log(current);
        GameObject connector = new GameObject();

        MeshFilter meshFilter = connector.AddComponent<MeshFilter>();
        meshFilter.mesh = _connectorMesh;

        MeshRenderer meshRenderer = connector.AddComponent<MeshRenderer>();
        meshRenderer.material = _ConnectorMaterial;

        connector.transform.position = prop._wire.transform.position;

        connector.transform.SetParent(transform);
        connector.name = "Connector";
        connector.transform.localScale = new Vector3(prop._width, prop._width, prop._width) * _connectorWidth;


        if (current != _wires.Length - 1)
        {
            Transform o1 = _wires[current - 1]._wire.transform;
            Transform o2 = _wires[current + 1]._wire.transform;

            Vector3 dir1 = (o1.position - connector.transform.position).normalized;
            Vector3 dir2 = (o2.position - connector.transform.position).normalized;

            Vector3 forwardVector = (dir1 + dir2).normalized;

            Vector3 sideVector = Vector3.Cross(dir1, dir2);

            if (forwardVector.sqrMagnitude > 0.001f)
            {
                Vector3 upVector = (sideVector.sqrMagnitude > 0.001f) ? sideVector : Vector3.up;

                connector.transform.rotation = Quaternion.LookRotation(forwardVector, upVector);
            }
        }
        else
        {
            
            Transform o1 = _wires[current]._wire.transform;
            Transform o2 = _wires[current - 1]._wire.transform;

            Vector3 direction = (o1.position - o2.position).normalized;
            Debug.Log(direction);
            if (direction != Vector3.zero)
            {
                connector.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                Debug.DrawRay(connector.transform.position, direction, Color.green, 100);
            }
        }
    }

    private void CreateWire(WireProp prop, int current)
    {
        Vector3 dist = _wires[current + 1]._wire.transform.position - _wires[current]._wire.transform.position;
        Vector3 pos = _wires[current]._wire.transform.position + dist / 2;
        //Debug.Log($"Pos1: {_wires[current + 1]._wire.transform.position} | Pos2: {_wires[current]._wire.transform.position} | pos = {pos}");

        GameObject wire = new GameObject();

        MeshFilter meshFilter = wire.AddComponent<MeshFilter>();
        meshFilter.mesh = _wireMesh;

        MeshRenderer meshRenderer = wire.AddComponent<MeshRenderer>();
        meshRenderer.material = _wireMaterial;

        wire.transform.SetParent(transform);
        wire.name = "wire";
        wire.transform.position = pos;
        wire.transform.localScale = new Vector3(prop._width * _wireWidth, prop._width * _wireWidth, Vector3.Distance(_wires[current + 1]._wire.transform.position, _wires[current]._wire.transform.position));

        Vector3 direction = dist.normalized;
        wire.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 0, 45);
    }

    private void OnDrawGizmos()
    {
        if (_wires == null || _wires.Length < 2 || !_debugLine) return;
        int current = 0;
        foreach (WireProp prop in _wires)
        {
            Gizmos.DrawLine(_wires[current]._wire.transform.position, _wires[current + 1]._wire.transform.position);
            if (current < _wires.Length - 2) current++;
            else return;
        }
    }
}
