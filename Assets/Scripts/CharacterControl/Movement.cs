using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private Vector3 _startSize;
    private Vector3 _crouchSize;

    private float _speedMultiply = 0.05f;
    private float _speed;
    private float _currentSpeed;
    private float _distance—overed;
    private float _hAxis; 
    private float _vAxis;

    private bool _ray1, _ray2, _ray3, _ray4, _ray5;
    public bool _isGrounded, _isSprint, _isCrouch, _canUp;

    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _distanceToStep;
    [SerializeField] private float _distanceToLanding;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private Vector3 _savedPosition;

    private RaycastHit _hit;

    void Start()
    {
        _startSize = transform.localScale;
        _crouchSize = new Vector3(transform.localScale.x, transform.localScale.y/2, transform.localScale.z);
        _speed = _walkSpeed;
        _rigidbody = GetComponent<Rigidbody>();

        InputReceiver.Instance.HorizontalAxis += HAxisUpdate;
        InputReceiver.Instance.VerticalAxis += VAxisUpdate;
        InputReceiver.Instance.Jump += Jump;
        InputReceiver.Instance.Crouch += Crouch;
        InputReceiver.Instance.Sprint += Sprint;
    }
    
    private void Update()
    {
        GroundCheck();
    }
    void FixedUpdate()
    {
        StepSoundCalculate();
        VelocityCalculate();
        StateCalculate();
    }

    private void StepSoundCalculate()
    {
        if (!_isGrounded) return;
        _currentSpeed = Mathf.Floor((transform.position - _savedPosition).magnitude*100)/100;
        _distance—overed += _currentSpeed;
        //Debug.Log(_distance—overed);
        if (_distance—overed >= _distanceToStep && _distance—overed < _distanceToLanding) 
        {
            //Debug.Log("try to play sound");
            _distance—overed = 0;
            if (UnityEngine.Random.Range(1,3) == 1) SoundService.Instance.PlaySound(SoundID.step1, _groundChecker.position, 0.5f);
            else SoundService.Instance.PlaySound(SoundID.step2, _groundChecker.position, 0.5f);


        }
        else if (_distance—overed >= _distanceToLanding) 
        {
            //Debug.Log("try to play sound");
            _distance—overed = 0;
            SoundService.Instance.PlaySound(SoundID.grounded, _groundChecker.position, 0.5f);
        }
        _savedPosition = transform.position;
    }

    private void GroundCheck()
    {
        Vector3 origin = _groundChecker.position + Vector3.up * 0.05f;
        Vector3 direction = Vector3.down;
        Vector3 direction2 = Vector3.up;

        float groundCheckDistance = 0.1f;
        float crouchCheckDistance = _startSize.y - 0.1f;
        //Debug.DrawRay(origin + Vector3.forward * gameObject.transform.localScale.x / 4, -direction * groundCheckDistance, Color.red);
        //Debug.DrawRay(origin - Vector3.forward * gameObject.transform.localScale.x / 4, -direction * groundCheckDistance, Color.red);
        //Debug.DrawRay(origin + Vector3.right * gameObject.transform.localScale.x / 4, -direction * groundCheckDistance, Color.red);
        //Debug.DrawRay(origin - Vector3.right * gameObject.transform.localScale.x / 4, -direction * groundCheckDistance, Color.red);
        Debug.DrawRay(transform.position, direction2 * crouchCheckDistance, Color.red, crouchCheckDistance);

        _ray1 = Physics.Raycast(origin + Vector3.forward * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);
        _ray2 = Physics.Raycast(origin - Vector3.forward * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);
        _ray3 = Physics.Raycast(origin + Vector3.right * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);
        _ray4 = Physics.Raycast(origin - Vector3.right * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);
        _ray5 = Physics.Raycast(origin, direction, out _hit, groundCheckDistance);

        _canUp = !Physics.Raycast(transform.position, direction2 * crouchCheckDistance, out _hit, crouchCheckDistance);

        _isGrounded = _ray1 == true || _ray2 == true || _ray3 == true || _ray4 == true || _ray5 == true;

        if (_isCrouch == false &&  _canUp == true)
        {
            Crouch(false);
        }
        else
        {
            Crouch(true);
        }

        //Debug.Log(_canUp);
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            //Debug.Log("try to play sound");
            SoundService.Instance.PlaySound(SoundID.jump, _groundChecker.position, 0.5f);
        }
    }
    private void Sprint(bool state)
    {
        _isSprint = state;
        Debug.Log(_speed);
    }
    private void Crouch(bool state)
    {
        if (_canUp == true)
        {
            _isCrouch = state;
        }

        if (_isCrouch == true)
        {
            transform.localScale = _crouchSize;
        }
        else if (_isCrouch == false && _canUp == true)
        {
            transform.localScale = _startSize;
        }
    }

    private void StateCalculate()
    {
        if (_isSprint == true) _speed = _sprintSpeed;
        if (_isCrouch == true) _speed = _crouchSpeed;
        if (_isCrouch == false && _isSprint == false) _speed = _walkSpeed;
    }
    private void HAxisUpdate(float value)
    {
        _hAxis = value;
    }
    private void VAxisUpdate(float value)
    {
        _vAxis = value;
    }

    private void VelocityCalculate()
    {
       Vector3 moveDirection = (transform.right * _hAxis + transform.forward * _vAxis).normalized * _speedMultiply;
        if (_isGrounded)
        {
            _velocity = moveDirection * _speed + transform.up * _rigidbody.velocity.y;
            _rigidbody.velocity = _velocity;
        }
    }
}
