using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int _index;
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))
            return;
        
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadData(GameData _gameData)
    {
    }

    public void SaveData(ref GameData _gameData)
    {
        if (_gameData.chaptersFinished.Count <= _index) return;
        
        _gameData.chaptersFinished[_index] = true;
    }
}
