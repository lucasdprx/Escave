using TMPro;
using UnityEngine;

public class TimerUIManager : MonoBehaviour, IDataPersistence
{
    [SerializeField]
    private int timerIndex;
    
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _timePassInLevel;
    private int _secondsPassInLevel;
    private int _minutesPassInLevel;
    private int _hoursPassInLevel;
    private int _millisecondsPassInLevel;

    private void Update()
    {
        _timePassInLevel += Time.deltaTime;
        _millisecondsPassInLevel = (int)(_timePassInLevel % (int)_timePassInLevel * 100);
        _secondsPassInLevel = (int)_timePassInLevel % 60;
        _minutesPassInLevel = (int)_timePassInLevel / 60;
        _minutesPassInLevel %= 60;
        _hoursPassInLevel = (int)_timePassInLevel / 3600;
        _timerText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", _hoursPassInLevel, _minutesPassInLevel,
            _secondsPassInLevel, _millisecondsPassInLevel);
    }

    public void LoadData(GameData _gameData)
    {
        if (_gameData.timers.Count <= timerIndex) return;
        
        _timePassInLevel = _gameData.timers[timerIndex];
    }

    public void SaveData(ref GameData _gameData)
    {
        if (_gameData.timers.Count <= timerIndex) return;
        
        _gameData.timers[timerIndex] = _timePassInLevel;
    }
}
