using UnityEngine;

public interface ILBPersistence
{
    void LoadData(LBData _gameData);
    void SaveData(ref LBData _gameData);
}
