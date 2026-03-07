using UnityEngine;

public class Graple : MonoBehaviour
{
    [SerializeField] private GameObject _hook;
    [SerializeField] private GameObject _hookVisual;
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody _rigitbody;
    [SerializeField] private Rigidbody _playerRigitbody;
    [SerializeField] private SphereCollider _collision;
    [SerializeField] private AudioSource _connect;
    [SerializeField] private AudioSource _throw;
    [SerializeField] private GameObject _rope;
    private byte _throwForce = 12;
    private float _speedMultiply = 200;
    private bool _hooked = false;
    private MeshRenderer _ropeRenderer;
    private MeshRenderer _hookRenderer;
    private GameObject _collision2;
    private Vector3 _hookLookPoint;
    private float _speed = 0.5f;
    private float _scrollAmount = 0.2f;
    private float _maxSpeed = 1f, _minSpeed = 0.3f;
    private bool en = false;
    public bool _canThrow = true;
    
    private void Start()
    {
        _playerRigitbody = _player.GetComponent<Rigidbody>();
        _ropeRenderer = _rope.GetComponent<MeshRenderer>();
        _hookRenderer = _hookVisual.GetComponent<MeshRenderer>();

        InputReceiver.Instance.HookThrow += HookThrow;
        InputReceiver.Instance.HookReturn += HookReturn;
        InputReceiver.Instance.HooksScroll += Scroll;
    }

    void LateUpdate()
    {
        RopeVisuals();
    }

    private void FixedUpdate()
    {
        if (_hooked == true)
        {
            _hook.transform.LookAt(_hookLookPoint);
            _playerRigitbody.AddForce((_hook.transform.position - _player.transform.position) * _speed * _speedMultiply);
        }
    }

    private void HookThrow()
    {
        if (_canThrow == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                _hook.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
                _rigitbody.isKinematic = false;
                _rigitbody.velocity = Camera.main.transform.forward * _throwForce;
                _collision.enabled = true;
                _hooked = false;
                _hookVisual.SetActive(true);
                _rope.SetActive(true);
                en = true;

                SoundService.Instance.PlaySound3D(SoundID.hookThrow, transform.position, 0.5f);
            }
        }
    }

    private void HookReturn()
    {
        _rigitbody.isKinematic = true;
        _hookVisual.SetActive(false);
        _rope.SetActive(false);
        _hooked = false;
        _rope.transform.localScale = new Vector3(0, 0, 0);
        en = false;

        SoundService.Instance.PlaySound3D(SoundID.hookReturn, transform.position, 0.5f);
    }

    private void Scroll(int direction)
    {
        _speed += direction * _scrollAmount;
        _speed = Mathf.Clamp(_speed, _minSpeed, _maxSpeed);
        Debug.Log(_speed);
        SoundService.Instance.PlaySound3D(SoundID.hookScroll, transform.position, 0.01f);
    }

    private void RopeVisuals()
    {
        Vector3 startPos = _hook.transform.position;
        Vector3 endPos = Camera.main.transform.position - Camera.main.transform.up * 0.2f + Camera.main.transform.right * -0.1f;
        _rope.transform.up = startPos - endPos;
        _rope.transform.localScale = new Vector3(0.01f, (_hook.transform.position - Camera.main.transform.position).magnitude / 2, 0.01f);
        _rope.transform.position = Vector3.Lerp(_rope.transform.position, new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, startPos.z + endPos.z) / 2f, Time.deltaTime * 1000);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundService.Instance.PlaySound3D(SoundID.hookCollide, transform.position, 0.5f);
        if (collision.gameObject.tag == "Hookable")
        {
            _rigitbody.isKinematic = true;
            _collision.enabled = false;
            ContactPoint contact = collision.contacts[0];
            _hookLookPoint = contact.point;
            _collision2 = collision.collider.gameObject;
            _hooked = true;
        }
    }
}
