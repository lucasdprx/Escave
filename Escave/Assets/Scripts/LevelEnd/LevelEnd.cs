using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour, IDataPersistence
{
    private int _index;
    private bool passedPassed;
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;

        passedPassed = true;
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        _index = SceneManager.GetActiveScene().buildIndex - 1;
    }

    public void LoadData(GameData _gameData)
    {
    }

    public void SaveData(ref GameData _gameData)
    {
        if (passedPassed)
        {
            _gameData.chapterUnlocked = true;
        }
    }
}
