using TMPro;
using UnityEngine;

public class TimerUIManager : MonoBehaviour, IDataPersistence
{
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
        if(_millisecondsPassInLevel < 0) _millisecondsPassInLevel = 0;
        _secondsPassInLevel = (int)_timePassInLevel % 60;
        _minutesPassInLevel = (int)_timePassInLevel / 60;
        _minutesPassInLevel %= 60;
        _hoursPassInLevel = (int)_timePassInLevel / 3600;
        //_timerText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", _hoursPassInLevel, _minutesPassInLevel,
            //_secondsPassInLevel, _millisecondsPassInLevel);
            
        _timerText.text = _hoursPassInLevel + "h" + _minutesPassInLevel + "m" + _secondsPassInLevel + "s" + _millisecondsPassInLevel;
    }

    public void LoadData(GameData _gameData)
    {
        _timePassInLevel = _gameData.timer;
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.timer = _timePassInLevel;
    }
}
