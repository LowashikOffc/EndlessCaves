using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerabob : MonoBehaviour
{
    private float timer;
    float bobSpeed = 0.18f;
    float bobAmount = 0.2f;
    float midpoint = 2f;

    void Update()
    {
        float waveslice = 0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cSharpConvesion = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateCh = waveslice * bobAmount;
            float totalAxes = Mathf.Abs(horizontal) * Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0f, 1f);
            translateCh = totalAxes * translateCh;
            cSharpConvesion.y = midpoint + translateCh;
        }
        else
        {
            cSharpConvesion.y = midpoint;
        }
        transform.localPosition = cSharpConvesion;
    }
}
