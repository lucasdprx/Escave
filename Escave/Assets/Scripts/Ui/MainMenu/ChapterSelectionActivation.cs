using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectionActivation : MonoBehaviour, IDataPersistence
{
    public Button chapterSelectionButton;
    
    public void LoadData(GameData _gameData)
    {
        chapterSelectionButton.interactable = _gameData.chapterUnlocked;
    }

    public void SaveData(ref GameData _gameData)
    {
    }
}
