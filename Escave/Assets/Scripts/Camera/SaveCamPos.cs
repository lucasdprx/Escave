using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCamPos : MonoBehaviour, IDataPersistence
{
    private int levelIndex;
    
    public void LoadData(GameData _gameData)
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (_gameData.cameraPos.Count > levelIndex)
        {
            transform.position = _gameData.cameraPos[levelIndex];
            Camera.main.transform.position = _gameData.cameraPos[levelIndex];
        }
    }

    public void SaveData(ref GameData _gameData)
    {
    
    }
}
