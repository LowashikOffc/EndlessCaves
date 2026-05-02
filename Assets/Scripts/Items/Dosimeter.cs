using UnityEngine;

public class Dosimeter : MonoBehaviour, IEquippable
{
    //private Animator _anim;
    //private static readonly int EquipTrigger = Animator.StringToHash("Equip");
    //private static readonly int UnequipTrigger = Animator.StringToHash("Unequip");

    private Rigidbody _rigidbody;
    private Collider _collision;

    private void Awake()
    {
        //_anim = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _collision = GetComponent<Collider>();
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
    public void ExecuteAction(string actionName)
    {
        Debug.Log($"Событие: {actionName}");
    }
}
