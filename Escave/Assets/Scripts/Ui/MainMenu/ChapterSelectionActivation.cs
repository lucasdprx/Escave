using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectionActivation : MonoBehaviour, IDataPersistence
{
    public Button chapterSelectionButton;
    
    public void LoadData(GameData _gameData)
    {
        chapterSelectionButton.interactable = false;
        
        if (_gameData.chaptersFinished.Count > 0 && _gameData.chaptersFinished[1] == true)
            chapterSelectionButton.interactable = true;
    }

    public void SaveData(ref GameData _gameData)
    {
    }
}
