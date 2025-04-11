using UnityEngine;

[System.Serializable]
public class OptionsData
{
    //----------Data to save----------
    public float mainVolume;
    public float musicVolume;
    public float sfxVolume;
    
    public bool fullScreen;

    public string resolution;
    //---------------------------------
    
    public OptionsData()
    {
        mainVolume = -10f;
        musicVolume = -10f;
        sfxVolume = -10f;
        
        resolution = "1920x1080";
        
        fullScreen = true;
    }
}
