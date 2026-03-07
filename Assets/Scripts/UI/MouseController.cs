using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController Instance { get; private set; }

    [SerializeField] private Texture2D _idle;
    [SerializeField] private Texture2D _click;

    

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

    void Start()
    {
        UpdateTexture(_idle);

        InputReceiver.Instance.MouseR += ClickR;
    }

    private void ClickR(bool pressed)
    {
        if (pressed) UpdateTexture(_click);
        else UpdateTexture(_idle);
    }

    private void UpdateTexture(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector3.zero + Vector3.up * 2, CursorMode.Auto);
    }

    public void UpdateState(bool lockState, bool visible)
    {
        CursorLockMode locked;
        if (lockState) locked = CursorLockMode.Locked;
        else locked = CursorLockMode.Confined;
        //Debug.Log("LockMode: "+locked+"; Visible: "+visible);
        Cursor.lockState = locked;
        Cursor.visible = visible;
    }

}
