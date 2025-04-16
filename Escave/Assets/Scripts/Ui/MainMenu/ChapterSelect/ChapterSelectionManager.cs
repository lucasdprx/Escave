using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChapterSelectionManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<Button> buttons;
    private AudioManager _audioManager;
    
    public void LoadChapters(string _levelName)
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(_levelName);
        _audioManager.PlaySound(AudioType.levelStart);
    }

    public void LoadData(GameData _gameData)
    {
        _audioManager = AudioManager.Instance;
        
        if (_gameData.chapterUnlocked.Count <= 0)
        {
            foreach (Button button in buttons)
            {
                _gameData.chapterUnlocked.Add(button.interactable);
            }
        }
        else
        {
            for (int i = 0; i < _gameData.chapterUnlocked.Count; i++)
            {
                if (_gameData.chapterUnlocked[i])
                {
                    buttons[i].interactable = true;
                }
            }
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        for (int i = 0; i < _gameData.chapterUnlocked.Count; i++)
        {
            _gameData.chapterUnlocked[i] = buttons[i].interactable;
        }
    }
}
