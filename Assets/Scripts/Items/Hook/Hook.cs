using System.Collections;
using UnityEngine;

public class Hook : MonoBehaviour, IEquippable
{
    private GameObject _player;
    private Rigidbody _playerRigitbody;
    private Camera _camera;
    private bool _isEquipped = false;

    [Header("Commponents")]
    [SerializeField] private GameObject _hook;
    [SerializeField] private Rigidbody _hookRigidbody;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Transform _hand;

    [Header("Rope Settings")]
    [SerializeField] private GameObject _rope;
    [SerializeField] private Transform _ropeStartPosition;
    [SerializeField] private Vector3 _ropeStartSize;

    private byte _throwForce = 12;
    private bool _hooked = false;
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
    }

    public void OnEquip()
    {
        PrepareHookState();
        // Подписываемся на ввод только когда предмет экипирован
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.HooksScroll += Scroll;
            InputReceiver.Instance.InputChange += Key;
        }
        InventoryManager.Instance.UpdateTransform(GetComponent<ItemObject>().vector3, GetComponent<ItemObject>().quaternion);
    }

    private void PrepareHookState()
    {
        _collider.enabled = false;
        _hookRigidbody.isKinematic = true;
        _hook.transform.localPosition = Vector3.zero;
        _hook.SetActive(true);
    }
    private void HookThrow()
    {
        if (!_canThrow) return;
        _hook.transform.SetParent(null);
        _hookRigidbody.isKinematic = false;
        _hookRigidbody.velocity = _camera.transform.forward * _throwForce;
        _collider.enabled = true;
        _rope.SetActive(true);
        SoundService.Instance.PlaySound3D(SoundID.hookThrow, transform.position, 0.5f);
    }

    private void HookReturn()
    {
        _hooked = false;
        _hook.transform.SetParent(transform);
        PrepareHookState();
        _rope.SetActive(false);
        SoundService.Instance.PlaySound3D(SoundID.hookReturn, transform.position, 0.5f);
    }
    public void OnUnequip()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange -= Key;
            InputReceiver.Instance.HooksScroll -= Scroll;
        }
        //Debug.Log($"{gameObject.name} убран в инвентарь");
        if (_hook != null)
        {
            _hook.transform.SetParent(null);
        }
        _hook.transform.SetParent(this.transform);
        PrepareHookState();
    }
    public void ExecuteAction(Actions action)
    {
        switch (action)
        {
            case Actions.Primary: HookThrow(); break;
            case Actions.Secondary: HookReturn(); break;
        }
    }
    public void Key(KeyCode key)
    {
        if (!_isEquipped) return;

        if (InventoryManager.Instance == null) return;

        GameObject currentItem = InventoryManager.Instance.GetCurrentEquippedItem();
        if (currentItem != this.gameObject)
        {
            return;
        }
        var activeSlot = InventoryManager.Instance.GetSelectedSlot();

        if (activeSlot == null || activeSlot._item == null)
        {
            //Debug.LogWarning("В руках нет предмета");
            return;
        }
        switch (key)
        {
            case KeyCode.Mouse0:
                ExecuteAction(Actions.Primary);
                break;
            case KeyCode.Mouse1:
                ExecuteAction(Actions.Secondary);
                break;
        }
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
        if (!_hook || _hand == null || _ropeStartPosition == null) return;
        RopeVisuals();
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
        _rope.transform.localScale = new Vector3(
            _ropeStartSize.x,
            Vector3.Distance(_ropeStartPosition.position, _hand.position),
            _ropeStartSize.z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        SoundService.Instance.PlaySound3D(SoundID.hookCollide, transform.position, 0.3f);
        if (collision.gameObject.tag == "Hookable")
        {

        }
    }
}
