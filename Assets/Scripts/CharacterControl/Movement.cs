using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _hAxis; 
    private float _vAxis;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    private float _accelerationFactor = 2f;
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

    private void GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red, transform.localScale.y / 6);
        if (Physics.SphereCast(transform.position, transform.localScale.x, Vector3.down, out _hit, transform.localScale.y / 6))
        {
            Debug.Log(_hit.collider.name);
        }
    }
    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
        GroundCheck();
        Vector3 direction = (transform.right * _hAxis + transform.forward * _vAxis).normalized;
        float currentSpeed = Vector3.Dot(_rigidbody.velocity, direction);
        float targetSpeed = _speed * _speedMultiply;
        float speedDifference = targetSpeed - currentSpeed;

        // ѕримен€ем силу дл€ достижени€ целевой скорости
        Vector3 acceleration = direction * speedDifference * _accelerationFactor;
        _rigidbody.AddForce(acceleration, ForceMode.Acceleration);
    }
}
