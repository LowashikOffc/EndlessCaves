using UnityEngine;

public class GenerationCollide : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //CaveGenerating.Instance.Generate();
        transform.GetComponent<Collider>().enabled = false;
    }
}
