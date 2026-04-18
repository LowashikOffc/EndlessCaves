using System;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxRayDistance;
    [SerializeField] private LayerMask _targetMask;
    private Vector3 _direction;
    private RaycastHit _hit;
    private GameObject _targetObject;
    private Item _targetItem;

    public event Action<String, String> UIChangeText;

    private void Start()
    {
        if (!GameObject.FindWithTag("Inventory")) return;
        _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        InputReceiver.Instance.Interact += UpdateInventory;
        InputReceiver.Instance.Drop += Drop;
    }

    private void OnDestroy()
    {
        InputReceiver.Instance.Interact -= UpdateInventory;
        InputReceiver.Instance.Drop -= Drop;
    }

    void Update()
    {
        _direction = _camera.transform.forward;
        if (Physics.Raycast(_camera.transform.position, _direction, out _hit, _maxRayDistance, _targetMask))
        {
            //Debug.Log(_hit.collider.gameObject.name);
            _targetObject = _hit.collider.gameObject;
            String text = _targetObject.name;
            String subText = "[Tab] to pick up";
            UIChangeText?.Invoke(text, subText);
        }
        else
        {
            String text = " ";
            String subText = " ";
            UIChangeText?.Invoke(text, subText);
            _targetObject = null;
        }
        //Debug.DrawRay(_camera.transform.position,_direction, new Color(0,1,0), _maxRayDistance);
    }

    private void Drop()
    {
        _inventory.Remove();
    }
    private void UpdateInventory()
    {
        if (_targetObject != null)
        {
            _targetItem = _targetObject.GetComponent<ItemConfig>().Item;
            _inventory.Add(_targetItem);
        }
    }
}
