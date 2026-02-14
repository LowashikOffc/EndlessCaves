using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraPos;
    private Vector3 _lastCameraPos;
    private float _speed;
    void Update()
    {
        float _sin = 0;
        _sin = Mathf.Sin(Time.time*2.5f) * 0.005f;
        //Debug.Log(_sin);
        transform.position = Vector3.Lerp(transform.position, _cameraPos.position + new Vector3(0, _sin, 0), Time.deltaTime * 20);
        _lastCameraPos = transform.position;
    }
}
