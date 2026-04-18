using System.Collections;
using UnityEngine;

public class TextLagging : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
         _camera = Camera.main;
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5));
            transform.rotation = Quaternion.Euler(0,0, Random.Range(-10, 10));

            if (Random.Range(0, 3) == 0)
            {
                yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
                transform.rotation = _camera.transform.rotation;
            }

            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            transform.rotation = Quaternion.Euler(0,0,0);
        }

    }
}
