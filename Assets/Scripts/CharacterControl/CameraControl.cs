using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class CameraControl : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public float cameraSmoothing;
    private float _smoothMultiply = 100f;

    public Transform Orient;
    public GameObject plr;

    private Quaternion savedRot;
    private float _xRotation, _yRotation;

    public bool canMove = true;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * 0.003f * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * 0.003f * sensY;

            _yRotation += mouseX;
            _xRotation -= mouseY;
            
            _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

            Camera.main.transform.rotation = Quaternion.Lerp(savedRot, Quaternion.Euler(_xRotation, _yRotation, 0), Time.deltaTime / cameraSmoothing * _smoothMultiply);
            savedRot = Camera.main.transform.rotation;
            Orient.rotation = Quaternion.Euler(0, _yRotation, 0);
            plr.transform.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
        Debug.DrawRay(Orient.position, transform.forward, Color.yellow);

    }
}
