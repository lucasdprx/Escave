using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenuPanel;
    public bool _isGamePaused;
    private bool _isSettingsMenuOpen;
    
    public static event Action OnRestartGame;
    
    public void ShowPausePanel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        PauseGame();
    }

    public void RestartGame()
    {
        PauseGame();
        OnRestartGame?.Invoke();
    }

    public void ShowSettings()
    {
        _settingsMenuPanel.SetActive(true);
        _isSettingsMenuOpen = true;
    }

    public void HideSettings()
    {
        _isSettingsMenuOpen = false;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    private void PauseGame()
    {
        if (_isSettingsMenuOpen)
            return;
        _isGamePaused = !_isGamePaused;
        gameObject.SetActive(_isGamePaused);

        Time.timeScale = _isGamePaused ? 0f : 1f;
    }
}
