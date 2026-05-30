using System.Collections;
using UnityEngine;

public class BatteryScript : MonoBehaviour, IEquippable
{
    public float _batteryEnergy;
    public bool _isUsing = false;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collision;
    private bool _isEquipped = false;

    private void Start()
    {
        _batteryEnergy = 1000;
        StartCoroutine(wait());
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.MouseL -= OnMouseLeft;
        }
    }
    private void Awake()
    {
        InputReceiver.Instance.MouseL += OnMouseLeft;
    }

    public void OnEquip()
    {
        _isEquipped = true;

        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null) _rigidbody.isKinematic = true; _rigidbody.useGravity = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        InventoryManager.Instance.UpdateTransform(GetComponent<ItemObject>().vector3, GetComponent<ItemObject>().quaternion);
    }
    public void OnUnequip()
    {
        _isEquipped = false;
    }
    private void OnMouseLeft(bool down)
    {
        if (!down) return;
        if (!_isEquipped) return;

        if (InventoryManager.Instance == null) return;

        GameObject currentItem = InventoryManager.Instance.GetCurrentEquippedItem();
        if (currentItem != this.gameObject) return;

        var activeSlot = InventoryManager.Instance.GetSelectedSlot();
        if (activeSlot == null || activeSlot._item == null) return;

        ExecuteAction(Actions.Primary);
    }
    public void ExecuteAction(Actions action)
    {
        Debug.Log($"�������: {action}");
    }

    IEnumerator wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (_isUsing == true)
            {
                _batteryEnergy = _batteryEnergy - 1;
            }
        }
    }
}
