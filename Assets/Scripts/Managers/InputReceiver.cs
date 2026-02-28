using System;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    public static InputReceiver Instance { get; private set; }

    public event Action<float> HorizontalAxis;
    public event Action<float> VerticalAxis;
    public event Action Jump;
    public event Action<int> SlotSelect;

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
        HorizontalAxis?.Invoke(Input.GetAxis("Horizontal"));
        VerticalAxis?.Invoke(Input.GetAxis("Vertical"));

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump?.Invoke();
        }
    }
}
