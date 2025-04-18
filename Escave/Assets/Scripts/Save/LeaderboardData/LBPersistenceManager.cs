using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class LBPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] 
    [SerializeField] private string fileName;
    
    [FormerlySerializedAs("_gameData")]
    [HideInInspector] public LBData lbData;
    private List<ILBPersistence> _dataPersistenceObjects;
    private TimerFileHandler _dataFileDataHandler;
    public static LBPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one LBPersistenceManager in scene.");
        }
        instance = this;
        _dataFileDataHandler = new TimerFileHandler(Application.persistentDataPath, fileName);
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void Start()
    {
        
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewLB()
    {
        this.lbData = new LBData();
    }

    public void LoadGame()
    {
        this.lbData = _dataFileDataHandler.Load();
        
        if (this.lbData == null)
        {
            NewLB();
        }

        foreach (ILBPersistence _dataPersistenceObject in this._dataPersistenceObjects)
        {
            _dataPersistenceObject.LoadData(lbData);
        }
    }

    public void SaveGame()
    {
        foreach (ILBPersistence _dataPersistenceObject in this._dataPersistenceObjects)
        {
            _dataPersistenceObject.SaveData(ref lbData);
        }
        
        _dataFileDataHandler.Save(lbData);
    }

    private List<ILBPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<ILBPersistence> optionPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ILBPersistence>();
        
        return new List<ILBPersistence>(optionPersistenceObjects);
    }
}
