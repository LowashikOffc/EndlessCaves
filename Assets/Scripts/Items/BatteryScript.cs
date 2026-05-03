using System.Collections;
using UnityEngine;

public class BatteryScript : MonoBehaviour, IEquippable
{
    public float _batteryEnergy;
    public bool _isUsing = false;
    private Rigidbody _rigidbody;
    private Collider _collision;

    private void Start()
    {
        _batteryEnergy = 1000;
        StartCoroutine(wait());
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collision = GetComponent<Collider>();
        InputReceiver.Instance.InputChange += Key;
    }

    public void OnEquip()
    {
        Debug.Log($"{gameObject.name} экипирован");
        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null) _rigidbody.isKinematic = true; _rigidbody.useGravity = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void OnUnequip()
    {
        Debug.Log($"{gameObject.name} убран в инвентарь");
    }
    public void Key(KeyCode key)
    {
        var activeSlot = InventoryManager.Instance.GetSelectedSlot();

        if (activeSlot == null || activeSlot._item == null)
        {
            Debug.LogWarning("В руках нет предмета");
            return;
        }
        switch (key)
        {
            case KeyCode.Mouse0:
                ExecuteAction(Actions.Primary);
                break;
        }
    }
    public void ExecuteAction(Actions action)
    {
        Debug.Log($"Событие: {action}");
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
