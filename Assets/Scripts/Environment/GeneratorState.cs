using System.Collections;
using TMPro;
using UnityEngine;

public class GeneratorState : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject[] _tripods;
    [SerializeField] private GameObject[] _tutorials;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioSource _start;
    [SerializeField] private AudioSource _stop;
    [SerializeField] private AudioSource _try;
    [SerializeField] private TMP_Text _fuelText;
    [SerializeField] private TMP_Text _enabledText;
    [SerializeField] private float _interactDistance;
    [SerializeField] private float _fuel;
    private bool _canInteract = false;
    private bool _canInteractOverride = true;
    private bool _enabled = false;
    private bool _canTry = true;
    private int _interactsToEnable;
    private int _currentInteracts;

    private void Start()
    {
        StartCoroutine(LightOff());
        StartCoroutine(GeneratorLogic());
        if (InputReceiver.Instance == null) return;
        InputReceiver.Instance.Interact += Interact;
    }

    private void OnDestroy()
    {
        if (InputReceiver.Instance == null) return;
        InputReceiver.Instance.Interact -= Interact;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(_player.position, transform.position) <= _interactDistance)
        {
            _canInteract = true;
        }
        else
        {
            _canInteract = false;
        }
    }

    IEnumerator GeneratorLogic()
    {
        while (true)
        {
            GuiVisual();
            yield return new WaitForSeconds(1f);
            if (_enabled)
            {
                if (_fuel > 0) _fuel = Mathf.Floor((_fuel - 0.1f)*10)/10;
                else if (_fuel <= 0)
                {
                    _fuel = 0;
                    StartCoroutine(LightOff());
                    _enabled = false;
                }
            }
        }
    }

    IEnumerator LightOn()
    {
        _audio.PlayDelayed(4);
        _start.Play();
        yield return new WaitForSeconds(2);
        foreach (GameObject tripod in _tripods)
        {
            yield return new WaitForSeconds(Random.Range(0.02f,0.1f));
            tripod.GetComponent<LightState>().UpdateLight(true);
        }
        foreach (GameObject tutorial in _tutorials)
        {
            yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
            tutorial.GetComponent<Canvas>().enabled = true;
        }
        yield return new WaitForSeconds(1);
        _canInteractOverride = true;
    }

    IEnumerator LightOff()
    {
        _audio.Stop();
        _stop.Play();
        foreach (GameObject tripod in _tripods)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.05f));
            tripod.GetComponent<LightState>().UpdateLight(false);
        }
        foreach (GameObject tutorial in _tutorials)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.05f));
            tutorial.GetComponent<Canvas>().enabled = false;
        }
        yield return new WaitForSeconds(3);
        _canInteractOverride = true;
    }
    private void GuiVisual()
    {
        string a = "Выключен";
        if (_enabled) a = "Включен";
        else a = "Выключен";
        _fuelText.text = $"Топливо: {_fuel}";
        _enabledText.text = $"Состояние: {a}";
    }

    IEnumerator WaitToTryAgain()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
        _canTry = true;
    }

    private void Interact()
    {
        if (_canInteract && _canInteractOverride)
        {
            if (_fuel <= 0) _currentInteracts = 0;
            _interactsToEnable = Random.Range(4, 12);
            if (!_enabled)
            {
                if (_canTry == false) return;
                _canTry = false;
                _currentInteracts++;
                _try.Play();
                StartCoroutine(WaitToTryAgain());
            }
            else _currentInteracts = _interactsToEnable;
            Debug.Log($"{_currentInteracts},{_interactsToEnable}");
            if (_fuel > 0 && _currentInteracts >= _interactsToEnable)
            {
                _currentInteracts = 0;
                _enabled = !_enabled;
                _canInteractOverride = false;
                if (_enabled)
                {
                    StartCoroutine(LightOn());
                }
                if (!_enabled)
                {
                    StartCoroutine(LightOff());
                }
                //Debug.Log(_enabled);
            }
        }
    }
}
