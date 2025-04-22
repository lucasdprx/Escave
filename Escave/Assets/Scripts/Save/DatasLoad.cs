using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatasLoad : MonoBehaviour, ILBPersistence
{
    [Header("Hearts")]
    [SerializeField] private List<Image> hearts;
    
    [Header("Death")]
    [SerializeField] private TextMeshProUGUI deathCount;
    
    [SerializeField] private Button button;

    [Header("Time")]
    [SerializeField] private TextMeshProUGUI currentTime;
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private int levelIndex;

    private float tempBestTime;
    private float tempCurrentTime;
    private bool bestTimeSet;
    
    public void LoadData(GameData _gameData)
    {
        //first set state
        if (!_gameData.buttonStateSet)
        {
            MainMenuDataManager.instance.SaveGame();
            MainMenuDataManager.instance.Load();
            return;
        }
        
        #region Hearts
        for (int i = 0; i < _gameData.collectibles.Count; i++)
        {
            if (_gameData.collectibles[i])
            {
                hearts[i].color = Color.white;
            }
        }
        #endregion
        
        #region Death
        deathCount.text = _gameData.deathCount.ToString();
        #endregion
        
        SetCurrentTime(_gameData.timer);
        tempCurrentTime = _gameData.timer;

        button.interactable = _gameData.chapterUnlocked;
        
        SetBestTime();
    }

    private void SetCurrentTime(float _timePassInLevel)
    {
        float _millisecondsPassInLevel = (int)(_timePassInLevel % (int)_timePassInLevel * 100);
        if(_millisecondsPassInLevel < 0) _millisecondsPassInLevel = 0;
        float _secondsPassInLevel = (int)_timePassInLevel % 60;
        float _minutesPassInLevel = (int)_timePassInLevel / 60;
        _minutesPassInLevel %= 60;
        float _hoursPassInLevel = (int)_timePassInLevel / 3600;
        
        currentTime.text = "Current time : " + _hoursPassInLevel + "h" + _minutesPassInLevel + "m" + _secondsPassInLevel + "s" + _millisecondsPassInLevel;
    }

    private void SetBestTime()
    {
        if (!bestTimeSet)
        {
            bestTimeSet = true;
            return;
        }

        if (IsBestTime(tempCurrentTime, tempBestTime))
        {
            tempBestTime = tempCurrentTime;
            LBPersistenceManager.instance.SaveGame();
        }
        
        float _millisecondsPassInLevel = (int)(tempBestTime % (int)tempBestTime * 100);
        if(_millisecondsPassInLevel < 0) _millisecondsPassInLevel = 0;
        float _secondsPassInLevel = (int)tempBestTime % 60;
        float _minutesPassInLevel = (int)tempBestTime / 60;
        _minutesPassInLevel %= 60;
        float _hoursPassInLevel = (int)tempBestTime / 3600;
        
        bestTime.text = "Best time : " + _hoursPassInLevel + "h" + _minutesPassInLevel + "m" + _secondsPassInLevel + "s" + _millisecondsPassInLevel;
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.chapterUnlocked = button.interactable;
        _gameData.buttonStateSet = true;
    }

    private bool IsBestTime(float _currentTime, float _bestTime)
    {
        Debug.Log(_currentTime);
        Debug.Log(_bestTime);
        return _currentTime <= _bestTime;
    }

    public void LoadData(LBData _gameData)
    {
        if (_gameData.timers.Count <= levelIndex)
        {
            tempBestTime = 0;
        }
        else
        {
            tempBestTime = _gameData.timers[levelIndex];
        }
        
        SetBestTime();
    }

    public void SaveData(ref LBData _gameData)
    {
        if(_gameData.timers.Count > levelIndex)
            _gameData.timers[levelIndex] = tempBestTime;
    }
}
