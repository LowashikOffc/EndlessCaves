using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _hAxis; 
    private float _vAxis;
    private bool _isGrounded;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    private float _accelerationFactor = 2f;
    private float _airAccelerationFactor = 0.2f;
    private float _speedMultiply = 0.7f;

    private RaycastHit _hit;

    [SerializeField] private Vector3 _velocity;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        InputReceiver.Instance.HorizontalAxis += HAxisUpdate;
        InputReceiver.Instance.VerticalAxis += VAxisUpdate;
        InputReceiver.Instance.Jump += Jump;
    }

    private void Update()
    {

        GroundCheck();
    }

    private void GroundCheck()
    {
        Vector3 origin = transform.position - transform.up/2f;
        Vector3 direction = -transform.up / 5;

        Debug.DrawRay(origin, direction, Color.red);

        float groundCheckDistance = 0.05f;
        if (Physics.Raycast(origin, direction, out _hit, groundCheckDistance))
        {
            _isGrounded = true; // Нашел землю в пределах дистанции
        }
        else
        {
            _isGrounded = false; // Не нашел
        }
        Debug.Log(_isGrounded); 
    }
    private void Jump()
    {
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    private void HAxisUpdate(float value)
    {
        _hAxis = value;
    }
    private void VAxisUpdate(float value)
    {
        _vAxis = value;
    }

    void FixedUpdate()
    {
        Vector3 direction = (transform.right * _hAxis + transform.forward * _vAxis).normalized;
        float currentSpeed = Vector3.Dot(_rigidbody.velocity, direction);
        float targetSpeed = _speed * _speedMultiply;
        float speedDifference = targetSpeed - currentSpeed;

        // Разные коэффициенты ускорения на земле и в воздухе
        float currentAcceleration = _isGrounded ? _accelerationFactor : _airAccelerationFactor;
        Vector3 acceleration = direction * speedDifference * currentAcceleration;


        _rigidbody.AddForce(acceleration, ForceMode.Acceleration);

    }
}
