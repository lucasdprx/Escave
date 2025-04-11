using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _settingsMenuPanel;
    [SerializeField] private PlayerDeath _playerDeath;
    private bool _isGamePaused = false;
    


    private void Awake()
    {
        if (_pauseMenuCanvas == null)
        {
            Debug.LogError("Le Canvas du menu pause n'est pas assigné !");
        }
    }

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
        _pauseMenuCanvas.SetActive(_isGamePaused);

        if (_isGamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
