using UnityEngine;

public class PlayerItemsManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private PioletPickUp _pioletObject;
    [SerializeField] private HookPickUp _hookObject;
    public void LoadData(GameData _gameData)
    {
        if (_gameData.isPioletEnabled && _pioletObject != null)
        {
            _pioletObject.EnablingPiolet();
        }        
        if (_gameData.isHookEnabled && _hookObject != null)
        {
            _hookObject.EnablingHook();
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        
    }
}
