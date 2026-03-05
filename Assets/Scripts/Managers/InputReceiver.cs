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
    [SerializeField] KeyCode _interactButton = KeyCode.E;
    [SerializeField] KeyCode _dropButton = KeyCode.Q;

    public event Action<float> HorizontalAxis;
    public event Action<float> VerticalAxis;
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
    }

    private void Update()
    {
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
                            SlotSelect?.Invoke(i);
                        } 
                    }
                }
            }
        }
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    InputChange?.Invoke(keyCode);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(_jumpButton))Jump?.Invoke();
        if (Input.GetKeyDown(_flashlightButton)) Flashlight?.Invoke();
        if (Input.GetKeyDown(_interactButton)) Interact?.Invoke();
        if (Input.GetKeyDown(_dropButton)) Drop?.Invoke();

        if (Input.GetKeyDown(KeyCode.Mouse0)) HookThrow?.Invoke();
        if (Input.GetKeyDown(KeyCode.Mouse1)) HookReturn?.Invoke();

        if (Input.GetKeyDown(_sprintButton)) Sprint?.Invoke(true);
        else if (Input.GetKeyUp(_sprintButton)) Sprint?.Invoke(false);

        if (Input.GetKeyDown(_crouchButton)) Crouch?.Invoke(true);
        else if (Input.GetKeyUp(_crouchButton)) Crouch?.Invoke(false);

        if (Input.GetKeyDown(KeyCode.Mouse0)) MouseR?.Invoke(true);
        else if (Input.GetKeyUp(KeyCode.Mouse0)) MouseR?.Invoke(false);

        if (Input.GetKeyDown(KeyCode.Mouse1)) MouseL?.Invoke(true);
        else if (Input.GetKeyUp(KeyCode.Mouse1)) MouseL?.Invoke(false);

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
    }

    public void Rebind(string keyword, KeyCode keycode)
    {
        if (keyword == "forward") _upButton = keycode;
        else if (keyword == "left") _leftButton = keycode;
        else if (keyword == "back") _downButton = keycode;
        else if (keyword == "right") _rightButton = keycode;
        else if (keyword == "jump") _jumpButton = keycode;
        else if (keyword == "sprint") _sprintButton = keycode;
        else if (keyword == "crouch") _crouchButton = keycode;
        else if (keyword == "flashlight") _flashlightButton = keycode;
        else if (keyword == "zoom") _zoomButton = keycode;
        else if (keyword == "interact") _interactButton = keycode;
        else if (keyword == "drop") _dropButton = keycode;
    }

    public KeyCode GetKey(string keyword)
    {
        if (keyword == "forward") return _upButton;
        else if (keyword == "left") return _leftButton;
        else if (keyword == "back") return _downButton;
        else if (keyword == "right") return _rightButton;
        else if (keyword == "jump") return _jumpButton;
        else if (keyword == "sprint") return _sprintButton;
        else if (keyword == "crouch") return _crouchButton;
        else if (keyword == "flashlight") return _flashlightButton;
        else if (keyword == "zoom") return _zoomButton;
        else if (keyword == "interact") return _interactButton;
        else if (keyword == "drop") return _dropButton;
        return KeyCode.None;
    }
}