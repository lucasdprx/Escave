using System.Collections;
using TMPro;
using UnityEngine;

public class TimerUIManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private float _timePassInLevel;
    private int _secondsPassInLevel;
    private int _minutesPassInLevel;
    private int _hoursPassInLevel;

    private void Update()
    {
        _timePassInLevel += Time.deltaTime;
        _secondsPassInLevel = (int)_timePassInLevel % 60;
        _minutesPassInLevel = (int)_timePassInLevel / 60;
        _minutesPassInLevel %= 60;
        _hoursPassInLevel = (int)_timePassInLevel / 3600;
        _timerText.text = string.Format("{0:00}:{1:00}:{2:00}", _hoursPassInLevel, _minutesPassInLevel,
            _secondsPassInLevel);
    }

    public void LoadData(GameData _gameData)
    {
        _timePassInLevel = _gameData.inGameTimer;
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.inGameTimer = _timePassInLevel;
    }
}
