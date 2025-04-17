using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectionActivation : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData _gameData)
    {
        if (_gameData.chaptersFinished.Count <= 0) return;
        
        if (_gameData.chaptersFinished[0] == false)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void SaveData(ref GameData _gameData)
    {
    }
}
