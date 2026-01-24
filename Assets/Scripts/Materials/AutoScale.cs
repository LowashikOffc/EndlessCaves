using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
    public bool XY, XZ, YZ;
    public bool reverse;
    public float multiply;
    void Start()
    {
        float x = gameObject.transform.localScale.x;
        float y = gameObject.transform.localScale.y;
        float z = gameObject.transform.localScale.z;

        gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(x, y);

        if (reverse == false)
        {
            if (XY == true)
            {
                gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(x,y);
            }
            else if (XZ == true)
            {
                gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(x,z);
            }
            else if (YZ == true)
            {
                gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(y,z);
            }
        }
        else
        {
            if (XY == true)
            {
                gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(y, x);
            }
            else if (XZ == true)
            {
                gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(z, x);
            }
            else if (YZ == true)
            {
                gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(z, y);
            }
        }
        if (multiply != 0)
        {
            gameObject.GetComponent<Renderer>().material.mainTextureScale *= multiply;
        }
    }
}
