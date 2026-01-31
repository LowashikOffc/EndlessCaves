using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{
    public float _textureScale;
    void Start()
    {
        float x = gameObject.transform.localScale.x;
        float y = gameObject.transform.localScale.y;
        float z = gameObject.transform.localScale.z;

        gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(x, y);

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

        if (_textureScale != 0)
        {
            gameObject.GetComponent<Renderer>().material.mainTextureScale *= _textureScale;
        }
    }
}
