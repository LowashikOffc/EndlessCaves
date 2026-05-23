using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Croshair : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Image _currentImage;
    [SerializeField] private TMP_Text _currentText;
    private void Start()
    {
        _playerInteraction.InteractionImage += ChangeImageByEnum;
        _playerInteraction.InteractionText += ChangeText;
    }

    private void ChangeImageByEnum(ImageEnum _enum)
    {
        //Debug.Log(_enum);
        if (_enum == ImageEnum.Default) _currentImage.sprite = _sprites[0];
        if (_enum == ImageEnum.Interact) _currentImage.sprite = _sprites[1];
    }

    private void ChangeText(string _string)
    {
        _currentText.text = _string;
    }
}

public enum ImageEnum
{
    Default,
    Interact
}
