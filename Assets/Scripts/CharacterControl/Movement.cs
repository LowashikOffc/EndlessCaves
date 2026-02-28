using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float _hAxis; 
    private float _vAxis;
    private bool _ray1, _ray2, _ray3, _ray4;
    private bool _isGrounded;
    [SerializeField] private Transform _groundChecker;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    private float _speedMultiply = 0.05f;

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
        Vector3 origin = _groundChecker.position;
        Vector3 direction = Vector3.down;

        float groundCheckDistance = 0.1f;
        Debug.DrawRay(origin + Vector3.forward * gameObject.transform.localScale.x / 4, direction * groundCheckDistance, Color.red);
        Debug.DrawRay(origin - Vector3.forward * gameObject.transform.localScale.x / 4, direction * groundCheckDistance, Color.red);
        Debug.DrawRay(origin + Vector3.right * gameObject.transform.localScale.x / 4, direction * groundCheckDistance, Color.red);
        Debug.DrawRay(origin - Vector3.right * gameObject.transform.localScale.x / 4, direction * groundCheckDistance, Color.red);

        _ray1 = Physics.Raycast(origin + Vector3.forward * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);
        _ray2 = Physics.Raycast(origin - Vector3.forward * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);
        _ray3 = Physics.Raycast(origin + Vector3.right * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);
        _ray4 = Physics.Raycast(origin - Vector3.right * gameObject.transform.localScale.x/4, direction, out _hit, groundCheckDistance);

        if (_ray1 == true || _ray2 == true || _ray3 == true || _ray4 == true)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
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
        Vector3 moveDirection = (transform.right * _hAxis + transform.forward * _vAxis).normalized * _speedMultiply * _speed + transform.up * _rigidbody.velocity.y;
        if (_isGrounded)
        {
            _rigidbody.velocity =  moveDirection;
        }

    }
}
