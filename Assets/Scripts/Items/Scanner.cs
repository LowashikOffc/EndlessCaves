using UnityEngine;

public class Scanner : MonoBehaviour, IEquippable
{
    private Rigidbody _rigidbody;
    private Collider _collision;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collision = GetComponent<Collider>();
    }

    private void Start()
    {
        InputReceiver.Instance.InputChange += Key;
    }

    public void OnEquip()
    {
        Debug.Log($"{gameObject.name} экипирован");
        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null) _rigidbody.isKinematic = true; _rigidbody.useGravity = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        //if (_anim != null) _anim.SetTrigger(EquipTrigger);
    }
    public void OnUnequip()
    {
        Debug.Log($"{gameObject.name} убран в инвентарь");
        //if (_anim != null) _anim.SetTrigger(UnequipTrigger);

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
            case KeyCode.Mouse1:
                ExecuteAction(Actions.Secondary);
                break;
        }
    }
    public void ExecuteAction(Actions action)
    {
        Debug.Log($"Событие: {action}");
    }
}
