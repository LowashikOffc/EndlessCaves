using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dosimeter : MonoBehaviour, IEquippable
{
    private Rigidbody _rigidbody;
    private Collider _collision;
    private bool _isEquipped = false;
    private float _currentZivert;
    private string text;

    [SerializeField] private TMP_Text _gammaText;
    [SerializeField] private TMP_Text _mcZValue;

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
        Debug.Log($"{gameObject.name} экипирован");

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
        Debug.Log($"{gameObject.name} убран в инвентарь");

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
    }

    IEnumerator UpdateScreen()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            _mcZValue.text = text;
        }
    }

    private float Noise()
    {
        return Random.Range(0.04f,0.11f);
    }

    private void UpdateZivert()
    {
        text = (Mathf.Floor((_currentZivert + Noise())*100)/100).ToString();
        if (text == "0") text = "0,00";
        _currentZivert = 0;
    }

}