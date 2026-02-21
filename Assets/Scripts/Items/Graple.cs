using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Graple : MonoBehaviour
{
    [SerializeField] private GameObject _hook;
    [SerializeField] private GameObject _hookVisual;
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody _rigitbody;
    [SerializeField] private Rigidbody _playerRigitbody;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private SphereCollider _collision;
    [SerializeField] private AudioSource _connect;
    [SerializeField] private AudioSource _throw;
    [SerializeField] private GameObject _rope;
    [SerializeField] private byte _throwForce = 12;
    [SerializeField] private float _speedMultiply = 200;
    [SerializeField] private bool _hooked = false;
    [SerializeField] private bool _selected = true;
    private MeshRenderer _ropeRenderer;
    private MeshRenderer _hookRenderer;
    private GameObject _collision2;
    private float _speed = 500;
    private bool en = false;
    public bool _canThrow = true;

    private void Start()
    {
        _playerRigitbody = _player.GetComponent<Rigidbody>();
        _characterController = _player.GetComponent<CharacterController>();
        _ropeRenderer = _rope.GetComponent<MeshRenderer>();
        _hookRenderer = _hookVisual.GetComponent<MeshRenderer>();
    }
    void LateUpdate()
    {
        if (_selected == true)
        {
            //Debug.Log(1);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Debug.Log(2);
                if (_canThrow == true)
                {
                    //Debug.Log(3);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        //Debug.Log(4);
                        _hook.transform.position = Camera.main.transform.position + Camera.main.transform.forward * Time.deltaTime;
                        _rigitbody.isKinematic = false;
                        _rigitbody.velocity = Camera.main.transform.forward * _throwForce;
                        _collision.enabled = true;
                        _hooked = false;
                        //_playerRigitbody.useGravity = true;
                        _characterController.canMove = true;
                        _speed = 0.9f;
                        _hookVisual.SetActive(true);
                        _rope.SetActive(true);
                        _throw.Play();
                        en = true;
                        //Debug.Log(5);
                    }
                }
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                HookFire();
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (_speed < 3f)
                {
                    _speed += 0.5f;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (_speed > 0.5f)
                {
                    _speed -= 0.5f;
                }
            }
            Vector3 startPos = _hook.transform.position;
            Vector3 endPos = Camera.main.transform.position - Camera.main.transform.up * 0.2f + Camera.main.transform.right * -0.1f;
            _rope.transform.up = startPos - endPos;
            _rope.transform.localScale = new Vector3(0.01f, (_hook.transform.position - Camera.main.transform.position).magnitude / 2, 0.01f);
            _rope.transform.position = Vector3.Lerp(_rope.transform.position, new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, startPos.z + endPos.z) / 2f, Time.deltaTime * 1000);

        }
    }

    private void HookFire()
    {
        _rigitbody.isKinematic = true;
        _hookVisual.SetActive(false);
        _rope.SetActive(false);
        //_playerRigitbody.useGravity = true;
        _characterController.canMove = true;
        _hooked = false;
        _rope.transform.localScale = new Vector3(0, 0, 0);
        en = false;
        //Debug.Log(6);
    }

    private void FixedUpdate()
    {
        if (_hooked == true)
        {
            _hook.transform.forward = _collision2.gameObject.transform.position;
            _playerRigitbody.AddForce((_hook.transform.position - _player.transform.position).normalized * _speed * _speedMultiply);
            //_playerRigitbody.AddForce(_player.gameObject.transform.up * -9.8f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _connect.Play();
        if (collision.gameObject.tag == "Hookable")
        {
            _rigitbody.isKinematic = true;
            _collision.enabled = false;
            _collision2 = collision.collider.gameObject;
            _hooked = true;
            //_playerRigitbody.useGravity = false;
            _characterController.canMove = false;
        }
    }
}
