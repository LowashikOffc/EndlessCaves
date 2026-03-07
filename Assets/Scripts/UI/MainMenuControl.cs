using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _tutorialButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _achievementsButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _graphicsButton;
    [SerializeField] private Button _controlsButton;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _guiButton;

    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _achievementsMenu;

    [SerializeField] private GameObject _graphics;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _audio;
    [SerializeField] private GameObject _gui;

    void Start()
    {
        _playButton.onClick.AddListener(Play);
        _tutorialButton.onClick.AddListener(Tutorial);
        _settingsButton.onClick.AddListener(Settings);
        _achievementsButton.onClick.AddListener(Achievements);
        _exitButton.onClick.AddListener(Settings);

        _graphicsButton.onClick.AddListener(Graphics);
        _controlsButton.onClick.AddListener(Controls);
        _audioButton.onClick.AddListener(Audio);
    }

    private void Play()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress);
        MouseController.Instance.UpdateState(true, false);
        SceneLoader.Instance.LoadSceneByIndex(2);
    }

    private void Tutorial()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress);
        MouseController.Instance.UpdateState(true, false);
        SceneLoader.Instance.LoadSceneByIndex(3);
    }

    private void Settings()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress);
        _settingsMenu.SetActive(true);
        _achievementsMenu.SetActive(false);
    }

    private void Graphics()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress);
        _graphics.SetActive(true);
        _controls.SetActive(false);
        _audio.SetActive(false);
        return;
        _gui.SetActive(false);
    }

    private void Achievements()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(true);
    }
    private void Controls()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress);
        _graphics.SetActive(false);
        _controls.SetActive(true);
        _audio.SetActive(false);
        return;
        _gui.SetActive(false);
    }

    private void Audio()
    {
        SoundService.Instance.PlaySound(SoundID.buttonPress);
        _graphics.SetActive(false);
        _controls.SetActive(false);
        _audio.SetActive(true);
        return;
        _gui.SetActive(false);
    }
}
