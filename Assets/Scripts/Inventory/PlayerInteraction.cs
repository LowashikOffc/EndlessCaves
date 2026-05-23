using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _interactDistance;
    private Ray _ray;
    private RaycastHit _hit;

    public event Action<ImageEnum> InteractionImage;
    public event Action<string> InteractionText;

    private void Start()
    {
        InputReceiver.Instance.Interact += TryPickup;
        InputReceiver.Instance.CameraLookAngle += RaycastFromReceiver;
    }

    private void RaycastFromReceiver(Ray ray)
    {
        _ray = ray;
    }

    private void Update()
    {
        _hit = Raycast();
        if (_hit.collider != null && _hit.collider.CompareTag("Item"))
            HoverEnter();
        else
            HoverExit();
    }

    private void HoverEnter()
    {
        //Debug.Log("Enter");
        InteractionImage?.Invoke(ImageEnum.Interact);
        string cleanedName = System.Text.RegularExpressions.Regex.Replace(_hit.collider.name, @"[\(\[\{][^\(\)\[\]\{\}]*[\)\]\}]", "");
        InteractionText?.Invoke(cleanedName.Trim());
    }

    private void HoverExit()
    {
        //Debug.Log("Exit");
        InteractionImage?.Invoke(ImageEnum.Default);
        InteractionText?.Invoke("");
    }

    private void TryPickup()
    {
        if (_hit.collider == null) return;

        if (_hit.collider.TryGetComponent(out ItemObject itemWorld))
        {
            if (InventoryManager.Instance.TryAddItem(itemWorld._data, itemWorld._amount))
            {
                Destroy(_hit.collider.gameObject);
            }
        }
    }

    private RaycastHit Raycast()
    {
        RaycastHit hit;
        Physics.Raycast(_ray, out hit, _interactDistance);
        
        return hit;
    }
}