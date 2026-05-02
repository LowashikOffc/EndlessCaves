using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData _data;
    public int _amount = 1;

    public void OnPickedUp()
    {
        Destroy(gameObject);
    }
}
