// using System.Collections.Generic;
// using TMPro;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.Rendering;
// using UnityEngine.SceneManagement;
//
// public class LaederBoardScript : MonoBehaviour, ILBPersistence, IDataPersistence
// {
//     [SerializeField] private List<TextMeshProUGUI> m_LaederBoard;
//     private List<float> m_timersToDisplay = new();
//
//     private bool isGameData;
//     private bool isLbData;
//     
//     private GameData m_GameData;
//     private LBData m_LBData;
//
//     private void DisplayLaederBoard()
//     {
//         if (isGameData && isLbData)
//         {
//             if (m_GameData.timers.Count <= 0) return;
//         
//             m_timersToDisplay = m_LBData.timers;
//         
//             for (int i = 0; i < m_GameData.timers.Count; i++)
//             {
//                 if (m_GameData.chaptersFinished[i])
//                 {
//                     if(i >= m_timersToDisplay.Count)
//                         m_timersToDisplay.Add(m_GameData.timers[i]);
//                     else
//                     {
//                         if (m_timersToDisplay[i] > m_GameData.timers[i])
//                         {
//                             m_timersToDisplay[i] = m_GameData.timers[i];
//                         }
//                     }
//                 }
//             }
//         
//             for (int i = 0; i < m_timersToDisplay.Count; i++)
//             {
//                 float _timer = m_timersToDisplay[i];
//             
//                 int _milisecondsPassInLevel = (int)(_timer % (int)_timer * 100);
//                 int _secondsPassInLevel = (int)_timer % 60;
//                 int _minutesPassInLevel = (int)_timer / 60;
//                 _minutesPassInLevel %= 60;
//                 int _hoursPassInLevel = (int)_timer / 3600;
//             
//                 string _time = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", _hoursPassInLevel, _minutesPassInLevel,
//                     _secondsPassInLevel, _milisecondsPassInLevel);
//             
//                 string _finalString = m_LaederBoard[i].text.Split(":")[0].Trim() + " : " + _time;
//             
//                 m_LaederBoard[i].text = _finalString;
//             }
//         }
//         LBPersistenceManager.instance.SaveGame();
//     }
//     
//     public void LoadData(LBData _lbData)
//     {
//         m_LBData = _lbData;
//         isLbData = true;
//         DisplayLaederBoard();
//     }
//
//     public void SaveData(ref LBData _lbData)
//     {
//         _lbData.timers = m_timersToDisplay;
//     }
//
//     public void LoadData(GameData _gameData)
//     {
//         m_GameData = _gameData;
//         isGameData = true;
//         DisplayLaederBoard();
//     }
//
//     public void SaveData(ref GameData _gameData)
//     {
//         
//     }
// }