using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class ButtonsLibrary
{
    public Button _button;
    public TMP_Text _text;
    public string _startText;
    public Color _hoverEnterColor;
    public Color _hoverExitColor;
    public bool _animate;
}
public class UISelectVisuals : MonoBehaviour
{

    [SerializeField] private ButtonsLibrary[] _buttons;

    private TMP_Text _selectedText;
    private string _selectedStartText;

    void Start()
    {
        StartCoroutine(ArrowsPointing());

        foreach (ButtonsLibrary lib in _buttons)
        {

            lib._hoverExitColor = lib._text.color;
            lib._startText = lib._text.text;

            EventTrigger trigger = lib._button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = lib._button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { HoverEnter(lib); });
            trigger.triggers.Add(entryEnter);

            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { HoverExit(lib); });
            trigger.triggers.Add(entryExit);
        }
    }

    private void HoverEnter(ButtonsLibrary lib)
    {
        if (lib._animate == true)
        {
            _selectedText = lib._text;
            _selectedStartText = lib._startText;
            lib._text.text = "> " + lib._startText + " <";
        }
        lib._text.color = lib._hoverEnterColor;
    }

    private void HoverExit(ButtonsLibrary lib)
    {
        _selectedText = null;
        _selectedStartText = null;
        lib._text.text = lib._startText;
        lib._text.color = lib._hoverExitColor;
    }

    IEnumerator ArrowsPointing()
    {
        bool enabled = true;
        while (true)
        {
            yield return new WaitForSeconds(0.33f);
            if (_selectedText == null || _selectedStartText == null) continue;
            enabled = !enabled;
            if (enabled) _selectedText.text = "> " + _selectedStartText + " <";
            else _selectedText.text = _selectedStartText;
        }
    }
}
