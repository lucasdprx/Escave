using UnityEngine;

public class SaveCamPos : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData _gameData)
    {
        transform.position = _gameData.cameraPos;
        Camera.main.transform.position = _gameData.cameraPos;
    }

    public void SaveData(ref GameData _gameData)
    {
    
    }
}
