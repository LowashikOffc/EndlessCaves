using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonLibrary
{
    public Button _button;
    public TMP_Text _keyCodeText;
    public string _keyWord;
    public KeyCode _keyCode;
}

public class SelectKeyCode : MonoBehaviour
{
    [SerializeField] private ButtonLibrary[] _buttons;
    [SerializeField] private GameObject _screenBlock;

    //private Button _selectedButton;
    private TMP_Text _selectedText;
    private KeyCode _selectedkeyCode;
    private string _selectedKeyWord;
    private bool _isSelected = false;

    void Start()
    {
        InputReceiver.Instance.InputChange += RepaintButton;

        foreach (ButtonLibrary lib in _buttons)
        {
            lib._button.onClick.AddListener(() => OnAnyButtonClick(lib));
            lib._keyCode = InputReceiver.Instance.GetKey(lib._keyWord);
            lib._keyCodeText.text = "[" + lib._keyCode.ToString() + "]";
        }
    }

    private void RepaintButton(KeyCode keycode)
    {
        if (_isSelected == true)
        {
            _screenBlock.SetActive(false);
            string displayText = RenameKeyCode(keycode);
            _selectedText.text = "[" + displayText + "]";
            _selectedkeyCode = keycode;
            InputReceiver.Instance.Rebind(_selectedKeyWord, _selectedkeyCode);
        }

        _isSelected = false;
    }

    private string RenameKeyCode(KeyCode keycode)
    {
        if (keycode >= KeyCode.Alpha0 && keycode <= KeyCode.Alpha9)
        {
            int number = keycode - KeyCode.Alpha0;
            return number.ToString();
        }

        if (keycode >= KeyCode.Keypad0 && keycode <= KeyCode.Keypad9)
        {
            int number = keycode - KeyCode.Keypad0;
            return "Num " + number.ToString();
        }

        return keycode.ToString();
    }

    void OnAnyButtonClick(ButtonLibrary lib)
    {
        _screenBlock.SetActive(true);
        //_selectedButton = lib._button;
        _selectedKeyWord = lib._keyWord;
        _selectedText = lib._keyCodeText;
        _selectedkeyCode = lib._keyCode;

        _isSelected = true;
    }
}