using UnityEngine;

public class Dosimeter : MonoBehaviour, IEquippable
{
    private Rigidbody _rigidbody;
    private Collider _collision;
    private bool _isEquipped = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collision = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange -= Key;
        }
    }

    public void OnEquip()
    {
        if (_isEquipped) return;

        _isEquipped = true;
        Debug.Log($"{gameObject.name} экипирован");

        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange += Key;
        }

        InventoryManager.Instance.UpdateTransform(GetComponent<ItemObject>().vector3, GetComponent<ItemObject>().quaternion);
    }

    public void OnUnequip()
    {
        if (!_isEquipped) return;

        _isEquipped = false;
        Debug.Log($"{gameObject.name} убран в инвентарь");

        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange -= Key;
        }
    }

    public void Key(KeyCode key)
    {
        if (!_isEquipped) return;

        if (InventoryManager.Instance == null) return;

        GameObject currentItem = InventoryManager.Instance.GetCurrentEquippedItem();
        if (currentItem != this.gameObject)
        {
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
        Debug.Log($"Событие: {action} от {gameObject.name}");
    }
}