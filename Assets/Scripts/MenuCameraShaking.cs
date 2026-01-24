using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraShaking : MonoBehaviour
{
    private Vector3 savedCamPos;
    private Quaternion savedCamRot;
    private Vector3 pos1 = new Vector3(0.3f,2,-10.6f);
    private Vector3 pos2 = new Vector3(0f, 2, -10.6f);

    private float timer = 30;
    private bool rl = false;
    void FixedUpdate()
    {
        type2();
    }

    void type1()
    {
        savedCamPos = Camera.main.transform.position;
        if (rl == false)
        {
            Camera.main.transform.position = Vector3.Lerp(savedCamPos, pos1, 0.005f);
        }
        else
        {
            Camera.main.transform.position = Vector3.Lerp(savedCamPos, pos2, 0.005f);
        }
        if (timer > 0)
        {
            timer -= 0.1f;
        }
        else
        {
            timer = 30;
            rl = !rl;
        }
    }
    void type2()
    {
        savedCamPos = Camera.main.transform.position;
        savedCamRot = Camera.main.transform.rotation;
        float H = Input.mousePosition.x;
        float V = Input.mousePosition.y;
        Camera.main.transform.position = Vector3.Lerp(savedCamPos, new Vector3((H / 4000)+0.5f,2,-10.6f), 0.5f);
        Camera.main.transform.rotation = Quaternion.Lerp(savedCamRot, Quaternion.Euler((V / -200)+20,(H / 200) - 10, savedCamRot.z), 0.5f);
    }
}
