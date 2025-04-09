using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _settingsMenuPanel;
    private bool _isGamePaused = false;

    public void ShowPausePanel(InputAction.CallbackContext context)
    {
        PauseGame();
    }

    public void ResumeGame()
    {
        PauseGame();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
    }

    public void ShowSettings()
    {
        _settingsMenuPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void PauseGame()
    {
        _pauseMenuPanel.SetActive(!_pauseMenuPanel.activeSelf);
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else if (!_isGamePaused)
        {
            Time.timeScale = 1f;
        }
    }
}
