using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LaederBoardScript : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<TextMeshProUGUI> m_LaederBoard;

    private List<float> tempTimers;
    
    public void LoadData(GameData _gameData)
    {
        if (_gameData.timers.Count <= 0) return;
        if(_gameData.chaptersFinished.Count <= 0) return;
        if (_gameData.chaptersFinished[0] == false) return;

        tempTimers = _gameData.timers;
        
        for (int i = 0; i < tempTimers.Count; ++i)
        {
            if (_gameData.timers[i] == 0) return;
            if (_gameData.chaptersFinished[i] == false) return;
            
            float _timer = _gameData.timers[i];
            
            int _milisecondsPassInLevel = (int)(_timer % (int)_timer * 100);
            int _secondsPassInLevel = (int)_timer % 60;
            int _minutesPassInLevel = (int)_timer / 60;
            _minutesPassInLevel %= 60;
            int _hoursPassInLevel = (int)_timer / 3600;
            
            string _time = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", _hoursPassInLevel, _minutesPassInLevel,
                _secondsPassInLevel, _milisecondsPassInLevel);
            
            string _finalString = m_LaederBoard[i].text.Split(":")[0].Trim() + " : " + _time;
            
            m_LaederBoard[i].text = _finalString;
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.timers = tempTimers;
    }
}
