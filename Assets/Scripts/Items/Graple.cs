using System.Collections;
using UnityEngine;

public class Graple : MonoBehaviour, IEquippable
{
    [SerializeField] private GameObject _hook;
    [SerializeField] private Transform _ropeStartPosition;
    [SerializeField] private GameObject _hookVisual;
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody _rigitbody;
    [SerializeField] private Rigidbody _playerRigitbody;
    [SerializeField] private SphereCollider _collision;
    [SerializeField] private AudioSource _connect;
    [SerializeField] private AudioSource _throw;
    [SerializeField] private GameObject _rope;
    private Camera _camera;
    private byte _throwForce = 12;
    private float _speedMultiply = 200;
    private bool _hooked = false;
    private bool _canScroll = true;
    private MeshRenderer _ropeRenderer;
    private MeshRenderer _hookRenderer;
    private Vector3 _hookLookPoint;
    private float _speed = 0.5f;
    private float _scrollAmount = 0.2f;
    private float _maxSpeed = 1f, _minSpeed = 0.3f;
    public bool _canThrow = true;
    
    private void Start()
    {
        _camera = Camera.main;
        _playerRigitbody = _player.GetComponent<Rigidbody>();
        _ropeRenderer = _rope.GetComponent<MeshRenderer>();
        _hookRenderer = _hookVisual.GetComponent<MeshRenderer>();

        if (InputReceiver.Instance == null) return;
        InputReceiver.Instance.HookThrow += HookThrow;
        InputReceiver.Instance.HookReturn += HookReturn;
        InputReceiver.Instance.HooksScroll += Scroll;
    }

    public void ExecuteAction(string actionName)
    {
        switch (actionName)
        {
            case "Primary": HookThrow(); break;
            case "Secondary": HookReturn(); break;
        }
    }

    public void OnEquip()
    {

    }
    public void OnUnequip()
    {

    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance == null) return;
        InputReceiver.Instance.HookThrow -= HookThrow;
        InputReceiver.Instance.HookReturn -= HookReturn;
        InputReceiver.Instance.HooksScroll -= Scroll;
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
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                _hook.transform.position = _camera.transform.position + _camera.transform.forward * 1;
                _rigitbody.isKinematic = false;
                _rigitbody.velocity = _camera.transform.forward * _throwForce;
                _collision.enabled = true;
                _hooked = false;
                _hookVisual.SetActive(true);
                _rope.SetActive(true);

                SoundService.Instance.PlaySound3D(SoundID.hookThrow, transform.position, 0.5f);
            }
        }
    }

    private void HookReturn()
    {
        if (_hooked == false) return;
        _rigitbody.isKinematic = true;
        _hookVisual.SetActive(false);
        _rope.SetActive(false);
        _hooked = false;
        _rope.transform.localScale = new Vector3(0, 0, 0);

        SoundService.Instance.PlaySound3D(SoundID.hookReturn, transform.position, 0.5f);
    }

    private void Scroll(int direction)
    {
        if (_canScroll == true)
        {
            _canScroll = false;
            _speed += direction * _scrollAmount;
            _speed = Mathf.Clamp(_speed, _minSpeed, _maxSpeed);
            Debug.Log(_speed);
            SoundService.Instance.PlaySound3D(SoundID.ropePull, transform.position, 0.05f);
            StartCoroutine(ScrollWait());
        }
    }

    IEnumerator ScrollWait()
    {
        yield return new WaitForSeconds(0.1f);
        _canScroll = true;
    }
    private void RopeVisuals()
    {
        Vector3 startPos = _ropeStartPosition.position;
        Vector3 endPos = _camera.transform.position - _camera.transform.up * 0.2f + _camera.transform.right * -0.1f;
        _rope.transform.up = startPos - endPos;
        _rope.transform.localScale = new Vector3(0.007f, (_hook.transform.position - _camera.transform.position).magnitude / 2, 0.007f);
        _rope.transform.position = Vector3.Lerp(_rope.transform.position, new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, startPos.z + endPos.z) / 2f, Time.deltaTime * 1000);
    }


    private void OnCollisionEnter(Collision collision)
    {
        SoundService.Instance.PlaySound3D(SoundID.hookCollide, transform.position, 0.3f);
        if (collision.gameObject.tag == "Hookable")
        {
            _rigitbody.isKinematic = true;
            _collision.enabled = false;
            ContactPoint contact = collision.contacts[0];
            _hookLookPoint = contact.point;
            _hooked = true;
        }
    }
}
