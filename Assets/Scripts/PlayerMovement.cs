using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public UnityEngine.CharacterController cc;
    public Rigidbody rb;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera _cam;
    [SerializeField] private float Sensitivity;

    [SerializeField] private float speed, walk, run, crouch;

    private Vector3 crouchScale, normalScale;

    public bool isMoving, isCrouching, isRunning;

    private void Start()
    {
        speed = walk;
        crouchScale = new Vector3(1, .40f, 1);
        normalScale = new Vector3(1, 1, 1);
        cc = GetComponent<UnityEngine.CharacterController>();
        cc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, _cam.transform.rotation.eulerAngles.y, 0.0f);


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        cc.SimpleMove((forward + right) * speed);

        if (Input.GetKey(KeyCode.LeftShift) && isCrouching == false)
        {
            speed = run;
            isRunning = true;
        }
        //Crouch
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = crouch;
            player.transform.localScale = crouchScale;
        }
        else
        {
            RaycastHit hit;

            if (!Physics.Raycast(player.transform.position, transform.TransformDirection(player.transform.up), out hit, 1.4f))
            {
                isRunning = false;
                isCrouching = false;
                speed = walk;
                player.transform.localScale = normalScale;
            }

        }

        isMoving = cc.velocity.sqrMagnitude > 0.0f ? true : false;
    }
}