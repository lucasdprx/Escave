using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData _gameData);
    void SaveData(ref GameData _gameData);
}
