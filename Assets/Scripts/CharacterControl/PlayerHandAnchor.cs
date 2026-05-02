using UnityEngine;

public class PlayerHandAnchor : MonoBehaviour
{
    void Start()
    {
        if (PlayerEquipmentManager.Instance != null) PlayerEquipmentManager.Instance.RegisterHand(transform);
    }
}
