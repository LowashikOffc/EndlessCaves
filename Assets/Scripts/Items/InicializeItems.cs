using UnityEngine;

public class InicializeItems : MonoBehaviour
{

    private void Start()
    {
        UpdateAllItems();
    }
    private void UpdateAllItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject obj in items)
        {
            // Получаем компоненты один раз
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            Collider col = obj.GetComponent<Collider>();

            // Проверяем наличие Rigidbody
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            // Проверяем наличие Collider
            if (col != null)
            {
                col.enabled = true;
            }
        }
    }
}
