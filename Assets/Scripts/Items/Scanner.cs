using System.Collections;
using TMPro;
using UnityEngine;


public class Scanner : MonoBehaviour, IEquippable
{
    [Header("Scaner elements")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collision;
    [SerializeField] private Transform _scanPoint;
    [SerializeField] private float _scanDistance;

    [Header("Scaner upgrades")]
    [SerializeField] private float _scanSpeed = 1;

    [Header("Scaner sounds")]
    [SerializeField] private AudioSource _scanSound;
    [SerializeField] private AudioSource _scanStart;
    [SerializeField] private AudioSource _scanPart;
    private bool _isEquipped = false;
    private bool _isScanning = false;
    private int _currentScanPart = 0;
    private RaycastHit _hit;
    private GameObject _scannedObject;
    private ItemScanDataScript _scannedDataScript;

    [Header("Scaner GUI elements")]
    [SerializeField] private TMP_Text _weight;
    [SerializeField] private TMP_Text _info;
    [SerializeField] private TMP_Text _type;
    [SerializeField] private TMP_Text _scanText;
    [SerializeField] private TMP_Text _scanPartText;
    [SerializeField] private GameObject _base;

    private void OnDestroy()
    {
        InputReceiver.Instance.Scan -= StartScan;
        _isEquipped = false;
    }

    public void OnEquip()
    {
        _isScanning = false;
        _base.SetActive(true);
        _scanText.text = "";
        _scanPartText.text = "";
        _isEquipped = true;
        Debug.Log($"{gameObject.name} экипирован");
        if (_collision != null) _collision.enabled = false;
        if (_rigidbody != null) _rigidbody.isKinematic = true; _rigidbody.useGravity = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.Scan += StartScan;
        }
        InventoryManager.Instance.UpdateTransform(GetComponent<ItemObject>().vector3, GetComponent<ItemObject>().quaternion);
        TipManager.Instance.AddTip(Tips.Scan);
    }
    public void OnUnequip()
    {
        if (InputReceiver.Instance != null)
        {
            InputReceiver.Instance.Scan -= StartScan;
        }
        _isEquipped = false;
        TipManager.Instance.DeleteTip(Tips.Scan);
        Debug.Log($"{gameObject.name} убран в инвентарь");

    }
    public void Key(KeyCode key)
    {
        //if (!_isEquipped) return;

        //switch (key)
        //{
        //    case KeyCode.R:
        //        ExecuteAction(Actions.Primary);
        //        break;
        //    case KeyCode.Mouse1:
        //        ExecuteAction(Actions.Secondary);
        //        break;
        //}
    }
    public void ExecuteAction(Actions action)
    {
        Debug.Log($"Событие: {action}");
        if (action == Actions.Primary) StartScan();
    }

    private void StartScan()
    {
        if (!_isEquipped)
        {
            Debug.LogWarning("Item is not equipped");
            return;
        }
        if (_isScanning)
        {
            Debug.LogWarning("Item is scanning now, wait");
            return;
        }
        if (Physics.Raycast(_scanPoint.position, _scanPoint.forward, out _hit, _scanDistance))
        {
            _scannedObject = _hit.collider.gameObject;
            if (!_scannedObject) return;
            if (_scannedObject.GetComponent<ItemScanDataScript>())
            {
                _scannedDataScript = _scannedObject.GetComponent<ItemScanDataScript>();
                Debug.Log("Start scanning");
                _isScanning = true;
                SoundService.Instance.PlayCustomSound3D(_scanStart, transform.position, 1);
            }
            else
            {
                Debug.LogWarning("Nothing to scan!");
                return;
            }
        }
        else
        {
            Debug.LogWarning("Nothing to scan!");
            return;
        }
        Debug.Log("Scanning ended with result" +
            $" Object: {_scannedObject}" +
            $" Name: {_scannedDataScript.Data.Name}" +
            $" Weight: {_scannedDataScript.Data.Weight}" +
            $" Rare: {_scannedDataScript.Data.Rare}" +
            $" Cost: {_scannedDataScript.Data.Cost}");
        StartCoroutine(ScanProcess());
    }

    IEnumerator ScanProcess()
    {
        if (_scannedDataScript.isScanned == false)
        {
            _currentScanPart = 0;
            _base.SetActive(false);
            _scanText.text = "Сканирование объекта...";
            _currentScanPart++;
            _scanPartText.text = $"{_currentScanPart}/3";
            _scanSound.Play();
            SoundService.Instance.PlayCustomSound3D(_scanPart, transform.position, .1f);
            yield return new WaitForSeconds(ScanTime());

            _scanText.text = "Сканирование типа...";
            _currentScanPart++;
            _scanPartText.text = $"{_currentScanPart}/3";
            _scanSound.Play();
            SoundService.Instance.PlayCustomSound3D(_scanPart, transform.position, .1f);
            yield return new WaitForSeconds(ScanTime());

            _scanText.text = "Обработка информации...";
            _currentScanPart++;
            _scanPartText.text = $"{_currentScanPart}/3";
            _scanSound.Play();
            SoundService.Instance.PlayCustomSound3D(_scanPart, transform.position, .1f);
            yield return new WaitForSeconds(ScanTime());
        }
        else
        {
            _base.SetActive(false);
            _scanText.text = "Сканирование не требуется";
            yield return new WaitForSeconds(0.5f);
        }
        _scanSound.Stop();

        string addToWeight = "kB";
        float weight = _scannedDataScript.Weight;
        if (weight >= 900)
        {
            weight = weight / 1000;
            addToWeight = "Mb";
        }
        _weight.text = weight + " " + addToWeight;
        _info.text = _scannedDataScript.Data.Description;
        _type.text = _scannedDataScript.Data.Name;

        _scannedDataScript.isScanned = true;
        _isScanning = false;
        _base.SetActive(true);
        _scanText.text = "";
        _scanPartText.text = "";
        SoundService.Instance.PlayCustomSound3D(_scanPart, transform.position, .1f);
        _scannedObject = null;
    }

    private float ScanTime()
    {
        float time = _scannedDataScript.Data.Weight / 5 / (5 * 10) / _scanSpeed;
        return time + Random.Range(-0.5f,0.5f);
    }
}
