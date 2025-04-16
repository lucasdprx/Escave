using UnityEngine;

public class SaveLevel : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject _level1;
    private GameObject _currentLevel;

    public void LoadData(GameData _gameData)
    {
        if (_gameData.levelToLoad == null)
        {
            _gameData.levelToLoad = _level1;
            _level1.SetActive(true);
            _currentLevel = _level1;
        }
        else
        {
            _currentLevel = _gameData.levelToLoad;
            _currentLevel.SetActive(true);
        }
    }
    public void SaveData(ref GameData _gameData)
    {
        print(_currentLevel);
        _gameData.levelToLoad = _currentLevel != null ? _currentLevel : _level1;
    }
    public void SetCurrentLevel(GameObject level)
    {
        _currentLevel = level;
    }
}
