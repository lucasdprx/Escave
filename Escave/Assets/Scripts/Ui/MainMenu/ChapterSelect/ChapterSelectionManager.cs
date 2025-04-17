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
        
        if (_gameData.chaptersFinished.Count <= 0)
        {
            foreach (Button button in buttons)
            {
                _gameData.chaptersFinished.Add(button.interactable);
            }
        }
        else
        {
            for (int i = 1; i < _gameData.chaptersFinished.Count; i++)
            {
                if (_gameData.chaptersFinished[i - 1])
                {
                    buttons[i].interactable = true;
                }
            }
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        for (int i = 1; i < _gameData.chaptersFinished.Count; i++)
        {
            _gameData.chaptersFinished[i - 1] = buttons[i].interactable;
        }
    }
}
