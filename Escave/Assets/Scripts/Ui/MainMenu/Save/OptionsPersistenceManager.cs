using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OptionsPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] 
    [SerializeField] private string fileName;
    
    private OptionsData _optionData;
    private List<IOptionPersistence> _optionPersistenceObjects;
    private OptionFileDataHandler _optionFileDataHandler;
    public static OptionsPersistenceManager instance { get; private set; }

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
        _optionFileDataHandler = new OptionFileDataHandler(Application.persistentDataPath, fileName);
        this._optionPersistenceObjects = FindAllObjectPersistenceObjects();
        LoadOptions();
    }

    private void OnApplicationQuit()
    {
        SaveOptions();
    }

    public void NewOptions()
    {
        this._optionData = new OptionsData();
    }

    public void LoadOptions()
    {
        this._optionData = _optionFileDataHandler.Load();
        
        if (this._optionData == null)
        {
            NewOptions();
        }

        foreach (IOptionPersistence optionPersistenceObject in this._optionPersistenceObjects)
        {
            optionPersistenceObject.LoadOption(_optionData);
        }
    }

    public void SaveOptions()
    {
        foreach (IOptionPersistence optionPersistenceObject in this._optionPersistenceObjects)
        {
            optionPersistenceObject.SaveOption(ref _optionData);
        }
        
        _optionFileDataHandler.Save(_optionData);
    }

    private List<IOptionPersistence> FindAllObjectPersistenceObjects()
    {
        IEnumerable<IOptionPersistence> optionPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IOptionPersistence>();
        
        return new List<IOptionPersistence>(optionPersistenceObjects);
    }
}
