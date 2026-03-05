using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Moving : MonoBehaviour
{
    private UnityEngine.CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float speed;
    public float cameraSmoothing;
    private float playerSpeed = 5f;
    public float jumpHeight = 4f;
    public float gravityValue = -9.81f;
    public float Sensitivity = 600;
    private float X, Y;
    public bool isWalk, isCrouched, isSprint;
    public float walkSpeed, runSpeed, crouchSpeed ;
    private float csp;
    Quaternion savedRot;

    private void Start()
    {
        csp = crouchSpeed;
        controller = gameObject.AddComponent<UnityEngine.CharacterController>();
        StartCoroutine(speedCount());
    }

    void Update()
    {
        const float MIN_Y = -80.0f;
        const float MAX_Y = 80.0f;

        X += Input.GetAxis("Mouse X") * (Sensitivity * 0.01f);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * 0.01f);

        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;

        //move
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        controller.Move((forward + (right/1.5f)) * (playerSpeed * speed/4) * 0.04f);

        // jump
        if (Input.GetKeyDown(KeyCode.Space)&&isCrouched == false&&controller.isGrounded)
        {
            //RaycastHit hit;
            //if (Physics.Raycast(gameObject.transform.position, Vector3.down * 2,out hit, 1.08f))
            //{
                print(controller.isGrounded);
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue/5);
            //}
        }

        //sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprint = true;
            isWalk = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprint = false;
            isWalk = true;
        }

        if (isWalk == true)
        {
            playerSpeed = walkSpeed;
            RaycastHit hit;
            if (!Physics.Raycast(gameObject.transform.position, Vector3.up * 2, out hit, 1.4f))
            {
                transform.localScale = new Vector3(1, 1, 1);
                isCrouched = false;
            }
        }
        if (isSprint == true&&isCrouched == false)
        {
            playerSpeed = runSpeed;
        }
        if (isCrouched == true)
        {
            playerSpeed = crouchSpeed;
            transform.localScale = new Vector3(1, 0.3f, 1);
        }

        //crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouched = true;
            isWalk = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isWalk = true;
        }

        playerVelocity.y += gravityValue * 0.01f;
        controller.Move(playerVelocity * 0.05f);
        transform.rotation = Quaternion.Euler(Y/3, X, 0.0f);
        Camera.main.transform.rotation = Quaternion.Lerp(savedRot, Quaternion.Euler(Y, X, 0), 6 / cameraSmoothing);
        savedRot = Camera.main.transform.rotation;
        
    }

    IEnumerator speedCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            float speed = gameObject.GetComponent<Rigidbody>().velocity.z;
            print(Mathf.Abs(Mathf.Floor(speed)));
        }
    }

}
