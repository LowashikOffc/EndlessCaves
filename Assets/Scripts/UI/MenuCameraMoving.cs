using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMoving : MonoBehaviour
{
    private Vector3 savedCamPos;
    private Quaternion savedCamRot;
    [SerializeField] private Vector3 pos;
    [SerializeField] private float _moveScale;

    private float timer = 30;
    private bool rl = false;
    void Update()
    {
        savedCamPos = Camera.main.transform.position;
        savedCamRot = Camera.main.transform.rotation;
        float H = Input.mousePosition.x;
        float V = Input.mousePosition.y;
        Camera.main.transform.position = Vector3.Lerp(savedCamPos, new Vector3(H * _moveScale,V * _moveScale * pos.y, -10.6f) + pos, Time.deltaTime * 10);
        Camera.main.transform.rotation = Quaternion.Lerp(savedCamRot, Quaternion.Euler((V / -200)+20,(H / 200) - 10, savedCamRot.z), Time.deltaTime * 10);
    }
}
