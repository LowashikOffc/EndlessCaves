using UnityEngine;
using Assets;
using UnityEngine.Audio;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.TerrainTools;

public class CharacterController : MonoBehaviour
{
    #region Variables
    public StateMachine StateMachine;
    public MoveState Moving;
    public IdleState Standing;

    private float _currentSpeed;
    private float _speedMultiply = 0.25f;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckDistance;

    [SerializeField] private float _standScaleY;
    [SerializeField] private float _crouchScaleY;
    [SerializeField] private float _scaleXZ;

    public bool canMove = true;
    public bool canJump = true;
    public bool canCrouch = true;
    [SerializeField] private bool _canMoveOverride = true;

    [SerializeField] private bool _isCrouch, _isSprint;

    private Rigidbody _rigidbody;
    [SerializeField] private LayerMask Target;
    #endregion

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StateMachine = new StateMachine();
        Moving = new MoveState(StateMachine, this);
        Standing = new IdleState(StateMachine, this);
        StateMachine.Initialize(Standing);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CalculateGroundDistance();
    }
    private void CalculateGroundDistance()
    {
        _groundCheckDistance = transform.localScale.y;
    }

    private void Update()
    {
        HandleJump();
        HandleCrouch();
        HandleSprint();

        CheckGroundDistance();

        StateMachine.CurrentState.HandleInput();
        StateMachine.CurrentState.OnLogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.OnPhysicsUpdate();
    }

    public void Move(float forwardSpeed, float strafeSpeed)
    {
        if (!_canMoveOverride || !canMove || _rigidbody.isKinematic) return;

        float forwardFactor = (forwardSpeed != 0 && strafeSpeed != 0) ? 0.7071f : 1f;
        float strafeFactor = (forwardSpeed != 0 && strafeSpeed != 0) ? 0.7071f : 1f;

        forwardSpeed *= forwardFactor;
        strafeSpeed *= strafeFactor;

        Vector3 moveForce = (forwardSpeed * transform.forward + strafeSpeed * transform.right) * _currentSpeed * _speedMultiply;
        moveForce.y = 0;

        _rigidbody.AddForce(moveForce * 7, ForceMode.Impulse);

        Vector3 flatVel = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        if (flatVel.magnitude > _currentSpeed)
        {
            flatVel = flatVel.normalized * _currentSpeed;
            _rigidbody.velocity = new Vector3(flatVel.x, _rigidbody.velocity.y, flatVel.z);
        }
    }

    private void CheckGroundDistance()
    {
        if (!Physics.SphereCast(transform.position, 0.2f, Vector3.down, out _, _groundCheckDistance, Target))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space) && canJump && canMove)
        {
            if (Physics.SphereCast(transform.position, 0.2f, Vector3.down, out _, _groundCheckDistance, Target))
            {
                _rigidbody.AddForce(Vector3.up * _jumpForce);
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

    private void HandleCrouch()
    {
        if (!canCrouch) return;

        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isCrouch)
        {
            transform.position -= new Vector3(0, _groundCheckDistance/2, 0);
            _isCrouch = true;
            _currentSpeed = _crouchSpeed;
            transform.localScale = new Vector3(_scaleXZ, _crouchScaleY, _scaleXZ);
            CalculateGroundDistance();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (!Physics.Raycast(transform.position, Vector3.up, 0.75f, Target))
            {
                _isCrouch = false;
                transform.position += new Vector3(0, _groundCheckDistance/2, 0);
                transform.localScale = new Vector3(_scaleXZ, _standScaleY, _scaleXZ);
                _currentSpeed = _isSprint ? _sprintSpeed : _walkingSpeed;
                CalculateGroundDistance();
            }
        }
    }

    private void HandleSprint()
    {
        if (_isCrouch) return;
        //if (gameObject.GetComponent<PlayerStats>().Stamina < 1)
        //{
        //    isSprint = false;
        //    MovementSpeed = 0.015f;
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isSprint = true;
            _currentSpeed = _sprintSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isSprint = false;
            _currentSpeed = _walkingSpeed;
        }
    }
}