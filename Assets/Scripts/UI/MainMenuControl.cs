using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _tutorialButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _achievementsButton;
    [SerializeField] private Button _mineralsInfoButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _graphicsButton;
    [SerializeField] private Button _controlsButton;
    [SerializeField] private Button _mouseButton;
    [SerializeField] private Button _specialButton;
    [SerializeField] private Button _audioButton;
    [SerializeField] private Button _guiButton;

    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _achievementsMenu;
    [SerializeField] private GameObject _mineralsInfoMenu;

    [SerializeField] private GameObject _graphics;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _mouse;
    [SerializeField] private GameObject _special;
    [SerializeField] private GameObject _audio;
    [SerializeField] private GameObject _gui;

    void Start()
    {
        CloseAll();
        CloseAllMenu();
        _playButton.onClick.AddListener(Play);
        _tutorialButton.onClick.AddListener(Tutorial);
        _settingsButton.onClick.AddListener(Settings);
        _achievementsButton.onClick.AddListener(Achievements);
        _mineralsInfoButton.onClick.AddListener(MineralsInfo);
        _exitButton.onClick.AddListener(Settings);

        _graphicsButton.onClick.AddListener(Graphics);
        _controlsButton.onClick.AddListener(Controls);
        _audioButton.onClick.AddListener(Audio);
        _guiButton.onClick.AddListener(Interface);
        _mouseButton.onClick.AddListener(Mouse);
        _specialButton.onClick.AddListener(Special);
    }

    private void Play()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        MouseController.Instance.UpdateState(true, false);
        SceneLoader.Instance.LoadSceneByIndex(2);
    }

    private void Tutorial()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        MouseController.Instance.UpdateState(true, false);
        SceneLoader.Instance.LoadSceneByIndex(3);
    }

    private void Settings()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        _settingsMenu.SetActive(true);
        _mineralsInfoMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
    }

    private void Graphics()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        CloseAll();
        _graphics.SetActive(true);
    }

    private void Achievements()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        _settingsMenu.SetActive(false);
        _mineralsInfoMenu.SetActive(false);
        _achievementsMenu.SetActive(true);
    }
    private void MineralsInfo()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        _settingsMenu.SetActive(false);
        _mineralsInfoMenu.SetActive(true);
        _achievementsMenu.SetActive(false);
    }
    private void Controls()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        CloseAll();
        _controls.SetActive(true);
    }

    private void Audio()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        CloseAll();
        _audio.SetActive(true);
    }
    private void Interface()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        CloseAll();
        _gui.SetActive(true);
    }
    private void Mouse()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        CloseAll();
        _mouse.SetActive(true);
    }
    private void Special()
    {
        SoundService.Instance.PlaySound(SoundID.uiPress);
        CloseAll();
        _special.SetActive(true);
    }

    private void CloseAll()
    {
        _graphics.SetActive(false);
        _controls.SetActive(false);
        _audio.SetActive(false);
        _gui.SetActive(false);
        _mouse.SetActive(false);
        _special.SetActive(false);
    }
    private void CloseAllMenu()
    {
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _mineralsInfoMenu.SetActive(false);
    }
}
