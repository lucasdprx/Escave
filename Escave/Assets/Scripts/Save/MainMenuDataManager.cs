using System.Collections.Generic;
using UnityEngine;

public class MainMenuDataManager : MonoBehaviour
{
    [SerializeField] private List<DatasLoad> levelDatasToLoad;
    
    private GameData currentGameData;
    
    [Header("File infos")]
    [SerializeField] private List<string> fileName;
    
    private DataFileHandler _dataFileDataHandler;
    public static MainMenuDataManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one DataPersistenceManager in scene.");
        }
        instance = this;
        
        Load();
    }

    public void Load()
    {
        for (int i = 0; i < levelDatasToLoad.Count; i++)
        {
            _dataFileDataHandler = new DataFileHandler(Application.persistentDataPath, fileName[i]);
            LoadGame(levelDatasToLoad[i]);
        }
    }
    
    public void NewDatas()
    {
        this.currentGameData = new GameData();
    }
    
    public void LoadGame(DatasLoad _datasLoaded)
    {
        this.currentGameData = _dataFileDataHandler.Load();
        
        if (this.currentGameData == null)
        {
            NewDatas();
        }
        
        print(currentGameData);

        _datasLoaded.LoadData(currentGameData);
    }
    
    public void SaveGame()
    {
        for (int i = 0; i < levelDatasToLoad.Count; i++)
        {
            _dataFileDataHandler = new DataFileHandler(Application.persistentDataPath, fileName[i]);
            this.currentGameData = _dataFileDataHandler.Load();
            
            if (this.currentGameData == null)
            {
                NewDatas();
            }
            
            levelDatasToLoad[i].SaveData(ref currentGameData);
            _dataFileDataHandler.Save(currentGameData);
        }
    }
}
