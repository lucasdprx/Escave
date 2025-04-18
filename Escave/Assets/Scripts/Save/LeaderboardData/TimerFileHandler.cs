using System;
using System.IO;
using UnityEngine;

public class TimerFileHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    public TimerFileHandler(string _dataDirPath, string _dataFileName)
    {
        this._dataDirPath = _dataDirPath;
        this._dataFileName = _dataFileName;
    }

    public LBData Load()
    {
        string _fullPath = Path.Combine(_dataDirPath, _dataFileName);
        LBData _loadedData = null;
        if (File.Exists(_fullPath))
        {
            try
            {
                string _optionsToLoad = "";
                using (FileStream _stream = new FileStream(_fullPath, FileMode.Open))
                {
                    using (StreamReader _reader = new StreamReader(_stream))
                    {
                        _optionsToLoad = _reader.ReadToEnd();
                    }
                }
                
                _loadedData = JsonUtility.FromJson<LBData>(_optionsToLoad);
            }
            catch (Exception _e)
            {
                Debug.LogError(_e);
            }
        }
        return _loadedData;
    }

    public void Save(LBData _options)
    {
        string _fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));
            
            string _dataToStore = JsonUtility.ToJson(_options, true);

            using (FileStream _stream = new FileStream(_fullPath, FileMode.Create))
            {
                using (StreamWriter _writer = new StreamWriter(_stream))
                {
                    _writer.Write(_dataToStore);
                }
            }
        }
        catch (Exception _e)
        {
            Debug.LogError(_e);
        }
    }
}
