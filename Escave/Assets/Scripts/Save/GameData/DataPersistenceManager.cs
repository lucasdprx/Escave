using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] 
    [SerializeField] private string fileName;
    
    [FormerlySerializedAs("_gameData")]
    [HideInInspector] public GameData gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private DataFileHandler _dataFileDataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one DataPersistenceManager in scene.");
        }
        instance = this;
        _dataFileDataHandler = new DataFileHandler(Application.persistentDataPath, fileName);
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewOptions()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = _dataFileDataHandler.Load();
        
        if (this.gameData == null)
        {
            NewOptions();
        }

        foreach (IDataPersistence _dataPersistenceObject in this._dataPersistenceObjects)
        {
            _dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence _dataPersistenceObject in this._dataPersistenceObjects)
        {
            _dataPersistenceObject.SaveData(ref gameData);
        }
        
        _dataFileDataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> optionPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(optionPersistenceObjects);
    }
}
