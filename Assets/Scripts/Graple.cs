using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graple : MonoBehaviour
{
    [SerializeField] private GameObject _hook;
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody _rigitbody;
    [SerializeField] private SphereCollider _collision;
    [SerializeField] private AudioSource _connect;
    [SerializeField] private AudioSource _throw;
    [SerializeField] private GameObject _rope;
    private GameObject _collision2;
    [SerializeField] private byte _throwForce = 12;
    [SerializeField] private bool _hooked = false;
    private bool en = false;
    public bool _canThrow = true;
    [SerializeField] private bool _selected = true;
    private float _speed = 500;

    void Update()
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
                        _player.GetComponent<Rigidbody>().useGravity = true;
                        _player.GetComponent<CharacterController>().canMove = true;
                        _speed = 0.9f;
                        _rope.transform.position = new Vector3(0, 100, 0);
                        _rope.transform.localScale = new Vector3(0, 0, 0);
                        _throw.Play();
                        en = true;
                        //Debug.Log(5);
                    }
                }
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                _rigitbody.isKinematic = true;
                _hook.transform.position = new Vector3(0, 100, 0);
                _player.GetComponent<Rigidbody>().useGravity = true;
                _player.GetComponent<CharacterController>().canMove = true;
                _hooked = false;
                _rope.transform.position = new Vector3(0, 100, 0);
                _rope.transform.localScale = new Vector3(0, 0, 0);
                en = false;
                //Debug.Log(6);
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
            if (en == true)
            {
                Vector3 startPos = _hook.transform.position;
                Vector3 endPos = _player.transform.position;
                _rope.transform.position = new Vector3(startPos.x + endPos.x, startPos.y + endPos.y, startPos.z + endPos.z) / 2f;
                _rope.transform.up = startPos - endPos;
                _rope.transform.localScale = new Vector3(0.01f, (_hook.transform.position - _player.transform.position).magnitude / 2, 0.01f);
                _rope.GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(0.05f, startPos.y - endPos.y) * 4;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_hooked == true)
        {
            _hook.transform.forward = _collision2.gameObject.transform.position;
            _player.GetComponent<Rigidbody>().AddForce((_hook.transform.position - _player.transform.position).normalized * _speed * Time.deltaTime * 4500f + Vector3.down * 60);
            _player.GetComponent<Rigidbody>().AddForce(_player.gameObject.transform.up * -9.8f);
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
            _player.GetComponent<Rigidbody>().useGravity = false;
            _player.GetComponent<CharacterController>().canMove = false;
        }
    }
}
