using Game.SeniorEventBus;
using Game.SeniorEventBus.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _mainCharacter;
    [SerializeField] private GameObject _pauseCamera;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _speedometerPanel;
    [SerializeField] private GameObject _completedPanel;
    private bool _pauseGame;
    private bool _canPause = true;

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<EnablePause>(PauseState, 1);

        if (_shopPanel != null && _speedometerPanel != null)
        {
            _speedometerPanel.SetActive(false);
            _shopPanel.SetActive(false);
        }

        if(_completedPanel != null)
            _completedPanel.SetActive(false);

        _pauseCamera.SetActive(false);
        Time.timeScale = 1.0f;
        _gamePanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_canPause)
            {
                if (_pauseGame)
                {
                    Continue();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    private void PauseState(EnablePause pause)
    {
        _canPause = pause.State;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        _gamePanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(true);
        _pauseGame = true;
        _mainCharacter.SetActive(false);
        _pauseCamera.SetActive(true);

        if (_shopPanel != null && _speedometerPanel != null)
        {
            _speedometerPanel.SetActive(false);
            _shopPanel.SetActive(false);
        }
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        _gamePanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _pauseGame = false;
        _mainCharacter.SetActive(true);
        _pauseCamera.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Settings()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    public void SetLowSettings()
    {
        QualitySettings.SetQualityLevel(0, true);
    }

    public void SetMediumSettings()
    {
        QualitySettings.SetQualityLevel(2, true);
    }

    public void SetHighSettings()
    {
        QualitySettings.SetQualityLevel(5, true);
    }

    public void FullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = true;
    }

    public void WindowMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.fullScreen = false;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<EnablePause>(PauseState);
    }
}