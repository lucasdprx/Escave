using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatasLoad : MonoBehaviour
{
    [Header("Hearts")]
    [SerializeField] private List<Image> hearts;
    
    [Header("Death")]
    [SerializeField] private TextMeshProUGUI deathCount;
    
    [SerializeField] private Button button;
    
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

        button.interactable = _gameData.chapterUnlocked;
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.chapterUnlocked = button.interactable;
        _gameData.buttonStateSet = true;
    }
}
