using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public UnityEngine.CharacterController cc;
    public Rigidbody rb;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float Sensitivity;

    [SerializeField] private float speed, walk, run, crouch;

    private Vector3 crouchScale, normalScale;

    public bool isMoving, isCrouching, isRunning;

    private float X, Y;

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
        #region Camera Limitation Calculator
        //Camera limitation variables
        const float MIN_Y = -80.0f;
        const float MAX_Y = 80.0f;

        X += Input.GetAxis("Mouse X") * (Sensitivity * 0.01f);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * 0.01f);

        if (Y < MIN_Y)
            Y = MIN_Y;
        else if (Y > MAX_Y)
            Y = MAX_Y;
        #endregion
        transform.rotation = Quaternion.Euler(0.0f, X, 0.0f);
        cam.transform.rotation = Quaternion.Euler(Y, X, 0.0f);


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        cc.SimpleMove((forward + right) * speed);
        // Determines if the speed = run or walk
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

        // Detects if the player is moving.
        // Useful if you want footstep sounds and or other features in your game.
        isMoving = cc.velocity.sqrMagnitude > 0.0f ? true : false;
    }
}