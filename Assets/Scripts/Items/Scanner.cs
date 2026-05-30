using UnityEngine;

public class Prams
{
    public Minerals Minerals;
    public float Weight;
    public int Rarity;
}

public class Scanner : MonoBehaviour, IEquippable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collision;
    [SerializeField] private Transform _scanPoint;
    private bool _isEquipped = false;


    private void OnDestroy()
    {
        InputReceiver.Instance.InputChange -= Key;
    }

    public void OnEquip()
    {
        Debug.Log($"{gameObject.name} экипирован");
        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null) _rigidbody.isKinematic = true; _rigidbody.useGravity = false;
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
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange -= Key;
        }
        //Debug.Log($"{gameObject.name} убран в инвентарь");

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
            case KeyCode.Mouse1:
                ExecuteAction(Actions.Secondary);
                break;
        }
    }
    public void ExecuteAction(Actions action)
    {
        Debug.Log($"Событие: {action}");
        if (action == Actions.Primary) StartScan();
    }

    private void StartScan()
    {

    }

    private Prams GetParameters()
    {
        var parameters = new Prams();
        return parameters;
    }
}
