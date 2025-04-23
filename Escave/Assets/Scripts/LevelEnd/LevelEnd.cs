using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour, IDataPersistence
{
    private bool passedPassed;
    [SerializeField] private EndPanelScript endPanel;
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;

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
            endPanel.InitializeEndPanelScript(ref _gameData);
        }
    }
}
