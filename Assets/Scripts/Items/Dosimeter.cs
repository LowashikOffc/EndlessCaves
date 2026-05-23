using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dosimeter : MonoBehaviour, IEquippable
{
    private Rigidbody _rigidbody;
    private Collider _collision;
    [SerializeField] private Image _screen;
    private bool _isEquipped = false;
    private bool _isBlink = false;
    private float _currentZivert;
    private float _a;
    private string text;

    [SerializeField] private Color _screenColor;
    [SerializeField] private TMP_Text _mcZValue;
    
    void Start()
    {
        StartCoroutine(ScreenBlink());
        GameManager.Instance.SendDosimeter(gameObject);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collision = GetComponent<Collider>();
        StartCoroutine(UpdateScreen());
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange -= Key;
        }
    }

    public void OnEquip()
    {
        if (_isEquipped) return;

        _isEquipped = true;
        //Debug.Log($"{gameObject.name} экипирован");

        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange += Key;
        }

        InventoryManager.Instance.UpdateTransform(GetComponent<ItemObject>().vector3, GetComponent<ItemObject>().quaternion);
    }

    public void OnUnequip()
    {
        if (!_isEquipped) return;

        _isEquipped = false;
        //Debug.Log($"{gameObject.name} убран в инвентарь");

        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.InputChange -= Key;
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

        switch (key)
        {
            case KeyCode.Mouse0:
                ExecuteAction(Actions.Primary);
                break;
        }
    }

    public void ExecuteAction(Actions action)
    {
        Debug.Log($"Событие: {action} от {gameObject.name}");
    }

    public void AddRadiation(float rad)
    {
        //Debug.Log(rad);
        _currentZivert += rad;
    }

    private void FixedUpdate()
    {
        UpdateZivert();
        if (Random.Range(-_a, _a) >= 5 && Random.Range(0,2) == 0) SoundService.Instance.PlaySound3D(SoundID.DosienerClick, transform.position, 0.5f);
    }

    IEnumerator UpdateScreen()
    {
        while (true)
        {
            _mcZValue.text = text;
            yield return new WaitForSeconds(0.25f);

        }
    }
    IEnumerator ScreenBlink()
    {
        while (true)
        {
            float time = Mathf.Lerp(0.5f, 0.1f, Mathf.Clamp01(Mathf.Clamp(_a, 0, 50) / 80f));
            //Debug.Log(time);
            if (_a > 40) SoundService.Instance.PlaySound3D(SoundID.DosienerAlarm, transform.position, 0.2f);
            if (_isBlink) _screen.enabled = !_screen.enabled;
            yield return new WaitForSeconds(time);
        }
    }
    private float Noise()
    {
        return Random.Range(0.04f,0.11f);
    }

    private void UpdateZivert()
    {
        _a = _currentZivert;
        text = (Mathf.Floor((_currentZivert + Noise())*100)/100).ToString();
        if (text == "0") text = "0,00";
        float hue = Mathf.Clamp(0.24f - _currentZivert /200, 0, 0.24f);
        //Debug.Log(hue);
        if (_currentZivert > 30) _isBlink = true;
        else
        {
            _isBlink = false;
            _screen.enabled = true;
        }
        _screenColor = Color.HSVToRGB(hue, 1f, 0.5f);
        _screen.color = _screenColor;
        _currentZivert = 0;
    }

}