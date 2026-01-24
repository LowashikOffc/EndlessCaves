using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noncollide : MonoBehaviour
{
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
