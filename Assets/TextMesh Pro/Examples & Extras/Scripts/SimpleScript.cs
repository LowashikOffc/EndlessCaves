using UnityEngine;
using System.Collections;

public class SimpleScript : MonoBehaviour
{
    IEnumerator wait()
    {
        yield return new WaitForSeconds(27);
        gameObject.GetComponent<AudioSource>().time = 0;
    }

    private void Start()
    {
        StartCoroutine(wait());
    }
}
