using System.Collections;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody _playerRigitbody;
    private Camera _camera;

    [Header("Commponents")]
    [SerializeField] private GameObject _hook;
    [SerializeField] private Rigidbody _hookRigidbody;
    [SerializeField] private MeshCollider _collider;
    [SerializeField] private Transform _hand;

    [Header("Audio")]
    [SerializeField] private AudioSource _scanSound;
    [SerializeField] private AudioSource _scanStart;
    [SerializeField] private AudioSource _scanChangePart;

    [Header("Rope Settings")]
    [SerializeField] private GameObject _rope;
    [SerializeField] private Transform _ropeStartPosition;
    [SerializeField] private Vector3 _ropeStartSize;

    private byte _throwForce = 12;
    private bool _hooked = false;
    private float _scrollSpeed = 1;
    private float _pullForce = 1000;
    private float _maxPullSpeed = 2000;
    private bool _canThrow = true;
    
    private void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerRigitbody = _player.GetComponent<Rigidbody>();
            _camera = Camera.main;
            _ropeStartSize = _rope.transform.localScale;
            _hand = GameObject.FindGameObjectWithTag("Hand").transform;
        }
        InputReceiver.Instance.HooksScroll += Scroll;
        InputReceiver.Instance.MouseL += OnMouseLeft;
        InputReceiver.Instance.MouseR += OnMouseRight;
    }

    private void PrepareHookState()
    {
        _hook.transform.localPosition = Vector3.zero;
    }
    private void HookThrow()
    {
        //Debug.Log("throw");
        if (!_canThrow) return;
        _hooked = false;
        _hookRigidbody.isKinematic = false;
        _hook.transform.position = _hand.transform.position;
        _hookRigidbody.velocity = _camera.transform.forward * _throwForce;
        _collider.enabled = true;
        _rope.SetActive(true);
        SoundService.Instance.PlaySound3D(SoundID.hookThrow, transform.position, 0.5f);
    }

    private void HookReturn()
    {
        _hooked = false;
        _collider.enabled = false;
        _hookRigidbody.isKinematic = true;
        _rope.SetActive(false);
        SoundService.Instance.PlaySound3D(SoundID.hookReturn, transform.position, 0.5f);
    }

    public void ExecuteAction(Actions action)
    {
        //Debug.Log("action");
        switch (action)
        {
            case Actions.Primary: HookThrow(); break;
            case Actions.Secondary: HookReturn(); break;
        }
    }
    private void OnMouseLeft(bool down)
    {
        if (down) ExecuteAction(Actions.Primary);
    }
    private void OnMouseRight(bool down)
    {
        if (down) ExecuteAction(Actions.Secondary);
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance == null) return;
        InputReceiver.Instance.MouseL -= OnMouseLeft;
        InputReceiver.Instance.MouseR -= OnMouseRight;
        InputReceiver.Instance.HooksScroll -= Scroll;
    }

    void LateUpdate()
    {
        if (!_hook || _hand == null || _ropeStartPosition == null) return;
        RopeVisuals();
    }

    private void FixedUpdate()
    {
        if (_hooked) PullPlayer();
    }
    private void PullPlayer()
    {
        Vector3 direction = (_hook.transform.position - _player.transform.position).normalized;
        float distance = Vector3.Distance(_player.transform.position, _hook.transform.position);

        float forceMultiplier = Mathf.Clamp(distance / 5f, 0.5f, 2f);
        Vector3 force = direction * _pullForce * forceMultiplier;

        if (_playerRigitbody.velocity.magnitude < _maxPullSpeed)
        {
            _playerRigitbody.AddForce(force, ForceMode.Force);
        }
    }
    private void Scroll(int direction)
    {

    }

    IEnumerator ScrollWait()
    {
        yield return new WaitForSeconds(0.1f);
    }
    private void RopeVisuals()
    {
        float distance = Vector3.Distance(_ropeStartPosition.position, _hand.position);

        _rope.transform.localScale = new Vector3(
            _ropeStartSize.x,
            distance/2,
            _ropeStartSize.z);

        _rope.transform.position = (_ropeStartPosition.position + _hand.position) / 2f;

        Vector3 direction = (_hand.position - _ropeStartPosition.position).normalized;
        _rope.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        SoundService.Instance.PlaySound3D(SoundID.hookCollide, transform.position, 0.3f);
        if (collision.gameObject.tag == "Hookable")
        {
            _hooked = true;
            _hookRigidbody.isKinematic = true;  
        }
    }
}
