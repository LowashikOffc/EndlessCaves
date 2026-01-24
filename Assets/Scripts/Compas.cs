using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compas : MonoBehaviour
{
    private void LateUpdate()
    {
        var t = Camera.main.transform;
        transform.position = Camera.main.transform.position + t.forward * 0.04f + t.up * (-0.025f);
    }
}
