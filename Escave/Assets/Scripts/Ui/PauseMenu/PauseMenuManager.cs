using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenuPanel;
    [SerializeField] private PlayerDeath _playerDeath;
    private bool _isGamePaused;

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
        _playerDeath._isRestarting = true;
        _playerDeath.PlayerDie();
        _playerDeath._isRestarting = false;
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
        _isGamePaused = !_isGamePaused;
        gameObject.SetActive(_isGamePaused);

        Time.timeScale = _isGamePaused ? 0f : 1f;
    }
}
