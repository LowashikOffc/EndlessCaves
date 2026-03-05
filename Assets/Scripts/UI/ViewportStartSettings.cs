using UnityEngine;
using UnityEngine.UI;

public class ViewportStartSettings : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    void Start()
    {
        _scrollbar.value = 1;
    }
}
