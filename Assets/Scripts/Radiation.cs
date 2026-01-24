using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiation : MonoBehaviour
{
    public GameObject pl;
    public float radiationScale = 1;
    public float radiationDist = 3;
    public void Start()
    {
        pl.GetComponent<Dosimeter>().rad.Add(gameObject);
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f/radiationScale);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, pl.transform.position - transform.position, out hit, radiationDist))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    pl.GetComponent<Prams>().hp -= 1;
                }
            }
        }
    }
}
