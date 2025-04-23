using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour, IDataPersistence
{
    private bool passedPassed;
    [SerializeField] private EndPanelScript endPanel;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject tempPos;
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;
        
        other.transform.position = tempPos.transform.position;

        passedPassed = true;
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData _gameData)
    {
    }

    public void SaveData(ref GameData _gameData)
    {
        if (passedPassed)
        {
            _gameData.chapterFinished = true;
            playerInput.enabled = false;
            endPanel.InitializeEndPanelScript(ref _gameData);
        }
    }
}
