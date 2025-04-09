using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] 
    [SerializeField] private string fileName;
    
    private GameData _gameData;
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
    }

    private void Start()
    {
        _dataFileDataHandler = new DataFileHandler(Application.persistentDataPath, fileName);
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadOptions();
    }

    private void OnApplicationQuit()
    {
        SaveOptions();
    }

    public void NewOptions()
    {
        this._gameData = new GameData();
    }

    public void LoadOptions()
    {
        this._gameData = _dataFileDataHandler.Load();
        
        if (this._gameData == null)
        {
            NewOptions();
        }

        foreach (IDataPersistence _dataPersistenceObject in this._dataPersistenceObjects)
        {
            _dataPersistenceObject.LoadData(_gameData);
        }
    }

    public void SaveOptions()
    {
        foreach (IDataPersistence _dataPersistenceObject in this._dataPersistenceObjects)
        {
            _dataPersistenceObject.SaveData(ref _gameData);
        }
        
        _dataFileDataHandler.Save(_gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> optionPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IDataPersistence>();
        
        return new List<IDataPersistence>(optionPersistenceObjects);
    }
}
