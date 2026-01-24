using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToPlayer : MonoBehaviour
{
    void Update()
    {
        //RaycastHit hit;

        //if (Physics.Raycast(transform.position, (Camera.main.transform.position - transform.position), out hit, 10))
        //{
        //    if (hit.collider.gameObject.tag == "Player")
        //    {
        //        if ((Camera.main.transform.position - transform.position).magnitude <= 5f)
        //        {
        //            gameObject.SetActive(true);
        //        }
        //        else gameObject.SetActive(false);
        //    }
        //}

        gameObject.transform.rotation = new Quaternion(0, Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w);
    }
}
