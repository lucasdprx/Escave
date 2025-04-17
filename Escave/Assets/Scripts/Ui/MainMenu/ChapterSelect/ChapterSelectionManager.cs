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
        SceneManager.LoadScene(_levelName);
        _audioManager.PlaySound(AudioType.levelStart);
    }

    public void LoadData(GameData _gameData)
    {
        _audioManager = AudioManager.Instance;

        if(_gameData.chaptersFinished[0]) buttons[1].interactable = true;
    }

    public void SaveData(ref GameData _gameData)
    {
    }
}
