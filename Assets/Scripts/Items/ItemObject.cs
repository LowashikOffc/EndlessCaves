using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData _data;
    public int _amount = 1;
    public Vector3 vector3;
    public Quaternion quaternion;

    public void OnPickedUp()
    {
        Destroy(gameObject);
    }
}
