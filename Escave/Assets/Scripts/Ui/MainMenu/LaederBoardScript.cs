using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaederBoardScript : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<TextMeshProUGUI> m_LaederBoard;
    
    public void LoadData(GameData _gameData)
    {
        for (int i = 0; i < m_LaederBoard.Count; ++i)
        {
            //string _finalString = m_LaederBoard[i].text.Split(":")[0].Trim() + " : " + _gameData.timers[i];
        }
    }

    public void SaveData(ref GameData _gameData)
    {
    }
}
