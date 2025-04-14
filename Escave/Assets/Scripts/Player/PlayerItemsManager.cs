using System.Collections.Generic;
using UnityEngine;

public class PlayerItemsManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private PioletPickUp _pioletObject;
    [SerializeField] private HookPickUp _hookObject;
    public void LoadData(GameData _gameData)
    {
        if (_gameData.isPioletEnabled == true)
        {
            _pioletObject.EnablingPiolet();
        }        
        if (_gameData.isHookEnabled == true)
        {
            _hookObject.EnablingHook();
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        
    }
}
