using System.Collections.Generic;
using UnityEngine;

public class MainMenuDataManager : MonoBehaviour
{
    [SerializeField] private List<DatasLoad> levelDatasToLoad;
    
    private GameData currentGameData;
    
    [Header("File infos")]
    [SerializeField] private List<string> fileName;
    [SerializeField] private string chaptersUnlockedFileName;
    
    private DataFileHandler _dataFileDataHandler;
    private ChaptersFileHandler _chaptersFileHandler;
    public ChaptersFileData _chaptersFileData;
    public static MainMenuDataManager instance { get; private set; }

    private bool allowLevel;
    
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
        _chaptersFileHandler = new ChaptersFileHandler(Application.persistentDataPath, chaptersUnlockedFileName);
        
        for (int i = 0; i < levelDatasToLoad.Count; i++)
        {
            _dataFileDataHandler = new DataFileHandler(Application.persistentDataPath, fileName[i]);
            BeginLoadGame(levelDatasToLoad[i], i);
        }
    }

    private void NewDatas()
    {
        this.currentGameData = new GameData();
    }

    public void Reset(int _index)
    {
        currentGameData = new GameData();
        _dataFileDataHandler = new DataFileHandler(Application.persistentDataPath, fileName[_index]);
        _dataFileDataHandler.Save(currentGameData);
    }
    
    public void LoadGame(DatasLoad _datasLoaded)
    {
        this.currentGameData = _dataFileDataHandler.Load();
        
        if (this.currentGameData == null)
        {
            NewDatas();
        }

        _datasLoaded.LoadData(currentGameData);
    }

    private void BeginLoadGame(DatasLoad _datasLoaded, int _index)
    {
        this.currentGameData = _dataFileDataHandler.Load();
        
        if (this.currentGameData == null)
        {
            NewDatas();
        }
        
        _chaptersFileData = _chaptersFileHandler.Load();
        if (_chaptersFileData == null)
        {
            _chaptersFileData = new ChaptersFileData();
        }
        if (currentGameData.chapterFinished) {
            if(_chaptersFileData.chaptersUnlocked.Count - 1 > _index)
                _chaptersFileData.chaptersUnlocked[_index + 1] = true;
            else
                _chaptersFileData.chaptersUnlocked.Add(true);
        }
        
        SaveChapters();

        _datasLoaded.LoadData(currentGameData);
    }
    
    public void SaveGame()
    {
        for (int i = 0; i < levelDatasToLoad.Count; i++)
        {
            _dataFileDataHandler = new DataFileHandler(Application.persistentDataPath, fileName[i]);
            currentGameData = _dataFileDataHandler.Load();
            
            if (currentGameData == null)
            {
                NewDatas();
            }
            
            levelDatasToLoad[i].SaveData(ref currentGameData);
            _dataFileDataHandler.Save(currentGameData);
        }
    }

    private void SaveChapters()
    {
        _chaptersFileHandler.Save(_chaptersFileData);
    }
}
