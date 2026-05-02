using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    private Ray _ray;
    private void Start()
    {
        
        InputReceiver.Instance.Interact += TryPickup;
        InputReceiver.Instance.CameraLookAngle += RaycastFromReceiver;
    }

    private void RaycastFromReceiver(Ray ray)
    {
        _ray = ray;
    }

    private void TryPickup()
    {
        if (Physics.Raycast(_ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent(out ItemObject itemWorld))
            {
                // Метод TryAddItem из прошлого сообщения сам проверит вес
                if (InventoryManager.Instance.TryAddItem(itemWorld._data, itemWorld._amount))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}