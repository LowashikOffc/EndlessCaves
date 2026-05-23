using System;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    public static InputReceiver Instance { get; private set; }

    [SerializeField] KeyCode _upButton = KeyCode.W;
    [SerializeField] KeyCode _leftButton = KeyCode.A;
    [SerializeField] KeyCode _downButton = KeyCode.S;
    [SerializeField] KeyCode _rightButton = KeyCode.D;
    [SerializeField] KeyCode _jumpButton = KeyCode.Space;
    [SerializeField] KeyCode _sprintButton = KeyCode.LeftShift;
    [SerializeField] KeyCode _crouchButton = KeyCode.LeftControl;
    [SerializeField] KeyCode _flashlightButton = KeyCode.F;
    [SerializeField] KeyCode _zoomButton = KeyCode.Z;
    [SerializeField] KeyCode _actionButton = KeyCode.E;
    [SerializeField] KeyCode _dropButton = KeyCode.Q;

    private InputConfig _config;

    public event Action<float> HorizontalAxis;
    public event Action<float> VerticalAxis;
    public event Action<Ray> CameraLookAngle;
    public event Action Jump;
    public event Action Interact;
    public event Action Drop;
    public event Action Flashlight;
    public event Action HookThrow;
    public event Action HookReturn;
    public event Action<int> HooksScroll;
    public event Action<bool> Zoom;
    public event Action<bool> Crouch;
    public event Action<bool> Sprint;
    public event Action<int> SlotSelect;

    public event Action<KeyCode> InputChange;

    public event Action<bool> MouseR;
    public event Action<bool> MouseL;

    private Camera _camera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SettingsManager.instance.LoadSettings();
        Load();
    }

    private void Update()
    {
        if (_camera == null) _camera = Camera.main;
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(_leftButton))
            horizontal = -1f;
        if (Input.GetKey(_rightButton))
            horizontal = 1f;

        if (Input.GetKey(_downButton))
            vertical = -1f;
        if (Input.GetKey(_upButton))
            vertical = 1f;

        HorizontalAxis?.Invoke(horizontal);
        VerticalAxis?.Invoke(vertical);

        if (Input.anyKey)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsLetterOrDigit(c))
                {
                    string s = c.ToString();
                    for (int i = 1; i <= 9; i++)
                    {
                        if (s == i.ToString())
                        {
                            //InventoryManager.Instance.SetSelectedSlot(i);
                            SlotSelect?.Invoke(i);
                        }
                    }
                }
            }
        }
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    //Debug.Log($"KeyDown detected: {keyCode}");
                    InputChange?.Invoke(keyCode);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(_jumpButton))Jump?.Invoke();
        if (Input.GetKeyDown(_flashlightButton)) Flashlight?.Invoke();
        if (Input.GetKeyDown(_actionButton)) Interact?.Invoke();
        if (Input.GetKeyDown(_dropButton)) Drop?.Invoke();

        if (Input.GetKeyDown(KeyCode.Mouse0)) HookThrow?.Invoke();
        if (Input.GetKeyDown(KeyCode.Mouse1)) HookReturn?.Invoke();

        if (Input.GetKeyDown(_sprintButton)) Sprint?.Invoke(true);
        else if (Input.GetKeyUp(_sprintButton)) Sprint?.Invoke(false);

        if (Input.GetKeyDown(_crouchButton)) Crouch?.Invoke(true);
        else if (Input.GetKeyUp(_crouchButton)) Crouch?.Invoke(false);

        if (Input.GetKeyDown(KeyCode.Mouse0)) MouseL?.Invoke(true);
        else if (Input.GetKeyUp(KeyCode.Mouse0)) MouseL?.Invoke(false);

        if (Input.GetKeyDown(KeyCode.Mouse1)) MouseR?.Invoke(true);
        else if (Input.GetKeyUp(KeyCode.Mouse1)) MouseR?.Invoke(false);

        if (Input.GetKeyDown(_zoomButton)) Zoom?.Invoke(true);
        else if (Input.GetKeyUp(_zoomButton)) Zoom?.Invoke(false);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            HooksScroll?.Invoke(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            HooksScroll?.Invoke(-1);
        }

        CameraLookAngle?.Invoke(_camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)));
    }

    public void Rebind(settingsEnum keyEnum, KeyCode keycode)
    {
        switch (keyEnum)
        {
            case settingsEnum.frontKey:
                _upButton = keycode;
                break;

            case settingsEnum.leftKey:
                _leftButton = keycode;
                break;

            case settingsEnum.backKey:
                _downButton = keycode;
                break;

            case settingsEnum.rightKey:
                _rightButton = keycode;
                break;

            case settingsEnum.jumpKey:
                _jumpButton = keycode;
                break;

            case settingsEnum.sprintKey:
                _sprintButton = keycode;
                break;

            case settingsEnum.crouchKey:
                _crouchButton = keycode;
                break;

            case settingsEnum.flashlightKey:
                _flashlightButton = keycode;
                break;

            case settingsEnum.zoomKey:
                _zoomButton = keycode;
                break;

            case settingsEnum.actionKey:
                _actionButton = keycode;
                break;

            case settingsEnum.dropKey:
                _dropButton = keycode;
                break;
        }
        Save();
    }

    private void Save()
    {
        Debug.Log("Input Save");
        Settings.Instance._front = _upButton;
        Settings.Instance._left = _leftButton;
        Settings.Instance._back = _downButton;
        Settings.Instance._right = _rightButton;
        Settings.Instance._jump = _jumpButton;
        Settings.Instance._sprint = _sprintButton;
        Settings.Instance._crouch = _crouchButton;
        Settings.Instance._flashlight = _flashlightButton;
        Settings.Instance._zoom = _zoomButton;
        Settings.Instance._action = _actionButton;
        Settings.Instance._drop = _dropButton;
        SettingsManager.instance.SaveSettings();
    }

    private void Load()
    {
        _upButton = Settings.Instance._front;
        _leftButton = Settings.Instance._left;
        _downButton = Settings.Instance._back;
        _rightButton = Settings.Instance._right;
        _jumpButton = Settings.Instance._jump;
        _sprintButton = Settings.Instance._sprint;
        _crouchButton = Settings.Instance._crouch;
        _flashlightButton = Settings.Instance._flashlight;
        _zoomButton = Settings.Instance._zoom;
        _actionButton = Settings.Instance._action;
        _dropButton = Settings.Instance._drop;
    }

    public KeyCode GetKey(settingsEnum keyEnum)
    {
        switch (keyEnum)
        {
            case settingsEnum.frontKey:
                return _upButton;

            case settingsEnum.leftKey:
                return _leftButton;

            case settingsEnum.backKey:
                return _downButton;

            case settingsEnum.rightKey:
                return _rightButton;

            case settingsEnum.jumpKey:
                return _jumpButton;

            case settingsEnum.sprintKey:
                return _sprintButton;

            case settingsEnum.crouchKey:
                return _crouchButton;

            case settingsEnum.flashlightKey:
                return _flashlightButton;

            case settingsEnum.zoomKey:
                return _zoomButton;

            case settingsEnum.actionKey:
                return _actionButton;

            case settingsEnum.dropKey:
                return _dropButton;
        }
        return KeyCode.None;
    }
}