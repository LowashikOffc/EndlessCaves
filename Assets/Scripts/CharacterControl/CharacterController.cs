using UnityEngine;
using Assets;
using UnityEngine.Audio;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Rendering.PostProcessing;

public class CharacterController : MonoBehaviour
{
    #region Variables
    public StateMachine StateMachine;
    public MoveState Moving;
    public IdleState Standing;

    public float MovementSpeed = 2f;
    public float SprintMultiplier = 1.4f;
    public float CrouchSpeed = 0.4f;
    public float JumpForce = 3000f;
    public float GroundCheckDistance = 0.5f;

    public bool canMove = true;
    public bool canJump = true;
    public bool canCrouch = true;
    public bool canMoveOverride = true;

    public bool isCrouch, isSprint;

    private Rigidbody rb;
    public LayerMask Target;
    #endregion

    Vignette vignette;
    public PostProcessVolume PPV;
    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        StateMachine = new StateMachine();
        Moving = new MoveState(StateMachine, this);
        Standing = new IdleState(StateMachine, this);
        StateMachine.Initialize(Standing);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {

        HandleJump();
        HandleCrouch();
        HandleSprint();

        StateMachine.CurrentState.HandleInput();
        StateMachine.CurrentState.OnLogicUpdate();

        if (!Physics.SphereCast(transform.position, 0.2f, Vector3.down, out _, GroundCheckDistance, Target))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    private void FixedUpdate()
    {

        StateMachine.CurrentState.OnPhysicsUpdate();
    }

    // ===== Движение =====
    public void Move(float forwardSpeed, float strafeSpeed)
    {
        if (!canMoveOverride || !canMove || rb.isKinematic) return;

        // Рассчитываем коэффициенты для выравнивания скорости
        float forwardFactor = (forwardSpeed != 0 && strafeSpeed != 0) ? 0.7071f : 1f; // 1/sqrt(2) для диагонального движения
        float strafeFactor = (forwardSpeed != 0 && strafeSpeed != 0) ? 0.7071f : 1f;

        // Применяем коэффициенты к скоростям
        forwardSpeed *= forwardFactor;
        strafeSpeed *= strafeFactor;

        // Рассчитываем силу движения
        Vector3 moveForce = (forwardSpeed * transform.forward + strafeSpeed * transform.right) * MovementSpeed * 0.3f;
        moveForce.y = 0;

        // Добавляем силу
        rb.AddForce(moveForce * 7, ForceMode.Impulse);

        // Ограничиваем максимальную скорость по XZ
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (flatVel.magnitude > MovementSpeed)
        {
            flatVel = flatVel.normalized * MovementSpeed;
            rb.velocity = new Vector3(flatVel.x, rb.velocity.y, flatVel.z);
        }
    }

    // ===== Прыжок =====
    private void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space) && canJump && canMove)
        {
            if (Physics.SphereCast(transform.position, 0.2f, Vector3.down, out _, GroundCheckDistance, Target))
            {
                rb.AddForce(Vector3.up * JumpForce);
                canJump = false;
                StartCoroutine(ResetJump());
            }
        }
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.24f);
        canJump = true;
    }

    // ===== Приседание =====
    private void HandleCrouch()
    {
        if (!canCrouch) return;

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouch)
        {
            GroundCheckDistance = 0.12f;
            transform.position -= new Vector3(0, 0.2f, 0);
            isCrouch = true;
            MovementSpeed = CrouchSpeed;
            transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (!Physics.Raycast(transform.position, Vector3.up, 0.75f, Target))
            {
                GroundCheckDistance = 0.4f;
                isCrouch = false;
                transform.position += new Vector3(0, 0.2f, 0);
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                MovementSpeed = isSprint ? SprintMultiplier : 0.2f;
            }
        }
    }

    // ===== Бег =====
    private void HandleSprint()
    {
        if (isCrouch) return;
        //if (gameObject.GetComponent<PlayerStats>().Stamina < 1)
        //{
        //    isSprint = false;
        //    MovementSpeed = 0.015f;
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprint = true;
            MovementSpeed = SprintMultiplier;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprint = false;
            MovementSpeed = 0.2f;
        }
    }
}