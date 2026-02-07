using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEvent : MonoBehaviour
{
    public AudioSource steps;
    public AudioSource drop;
    public GameObject pl;
    byte range = 80;
    IEnumerator Steps()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(300, 600));
            AudioSource.PlayClipAtPoint(steps.clip, new Vector3(pl.transform.position.x + Random.Range(-range, range), pl.transform.position.y + Random.Range(-range, range), pl.transform.position.z + Random.Range(-range, range)));
        }
    }
    IEnumerator Drops()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f,2));
            AudioSource.PlayClipAtPoint(drop.clip, new Vector3(pl.transform.position.x + Random.Range(-range, range), pl.transform.position.y + Random.Range(-range, range)/10, pl.transform.position.z + Random.Range(-range, range)));
            
        }
    }
    private void Start()
    {
        StartCoroutine(Steps());
        StartCoroutine(Drops());
    }
}
