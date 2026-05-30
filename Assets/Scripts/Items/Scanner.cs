using UnityEngine;

public class Scanner : MonoBehaviour, IEquippable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collision;
    private bool _isEquipped = false;


    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.MouseL -= OnMouseLeft;
            InputReceiver.Instance.MouseR -= OnMouseRight;
        }
    }

    public void OnEquip()
    {
        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null) _rigidbody.isKinematic = true; _rigidbody.useGravity = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.MouseL += OnMouseLeft;
            InputReceiver.Instance.MouseR += OnMouseRight;
        }
        InventoryManager.Instance.UpdateTransform(GetComponent<ItemObject>().vector3, GetComponent<ItemObject>().quaternion);
    }
    public void OnUnequip()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.MouseL -= OnMouseLeft;
            InputReceiver.Instance.MouseR -= OnMouseRight;
        }
    }
    private void OnMouseLeft(bool down)
    {
        if (down) HandleItemAction(Actions.Primary);
    }
    private void OnMouseRight(bool down)
    {
        if (down) HandleItemAction(Actions.Secondary);
    }
    private void HandleItemAction(Actions action)
    {
        if (!_isEquipped) return;
        if (InventoryManager.Instance == null) return;

        GameObject currentItem = InventoryManager.Instance.GetCurrentEquippedItem();
        if (currentItem != this.gameObject) return;

        var activeSlot = InventoryManager.Instance.GetSelectedSlot();
        if (activeSlot == null || activeSlot._item == null) return;

        ExecuteAction(action);
    }
    public void ExecuteAction(Actions action)
    {
        Debug.Log($"�������: {action}");
    }
}
