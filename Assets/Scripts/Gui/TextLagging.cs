using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLagging : MonoBehaviour
{
    public GameObject txt;
    private void Start()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 9));
            txt.transform.rotation = Quaternion.Euler(0,0, UnityEngine.Random.Range(-10, 10));
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.1f));
            txt.transform.rotation = Camera.main.transform.rotation;
        }

    }
}
