using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveCamPos : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData _gameData)
    {
        if ((Vector2)_gameData.cameraPos == Vector2.zero) return;
        
        transform.position = _gameData.cameraPos;
        Camera.main.transform.position = _gameData.cameraPos;
    }

    public void SaveData(ref GameData _gameData)
    {
    
    }
}
