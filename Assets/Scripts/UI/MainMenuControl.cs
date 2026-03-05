using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _tutorialButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _controlsButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _graphicsButton;
    [SerializeField] private Button _controlsButtonAlt;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _guiButton;

    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _graphics;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _audio;
    [SerializeField] private GameObject _gui;

    void Start()
    {
        _playButton.onClick.AddListener(Play);
        _tutorialButton.onClick.AddListener(Tutorial);
        _settingsButton.onClick.AddListener(Settings);
        _controlsButton.onClick.AddListener(Controls);
        _exitButton.onClick.AddListener(Settings);

        _graphicsButton.onClick.AddListener(Graphics);
        _controlsButtonAlt.onClick.AddListener(ControlsAlt);
    }

    private void Play()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress, transform.position);
        MouseController.Instance.UpdateState(true, false);
        SceneLoader.Instance.LoadSceneByIndex(2);
    }

    private void Tutorial()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress, transform.position);
        MouseController.Instance.UpdateState(true, false);
        SceneLoader.Instance.LoadSceneByIndex(3);
    }

    private void Settings()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress, transform.position);
        _settingsMenu.SetActive(true);
    }

    private void Graphics()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress, transform.position);
        _graphics.SetActive(true);
        _controls.SetActive(false);
        return;
        _audio.SetActive(false);
        _gui.SetActive(false);
    }

    private void Controls()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress, transform.position);
        _settingsMenu.SetActive(true);
        _graphics.SetActive(false);
        _controls.SetActive(true);
        return;
        _audio.SetActive(false);
        _gui.SetActive(false);
    }
    private void ControlsAlt()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress, transform.position);
        _graphics.SetActive(false);
        _controls.SetActive(true);
        return;
        _audio.SetActive(false);
        _gui.SetActive(false);
    }
}
