using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour, IOptionPersistence
{
    public Slider masterVol, sfxVol, musicVol;
    public AudioMixer mainAudioMixer;

    public Toggle fullscreenToggle;

    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVol", masterVol.value);
    }
    
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVol", musicVol.value);
    }
    
    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("SFXVol", sfxVol.value);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
    
    public void LoadOption(OptionsData _optionData)
    {
        masterVol.value = _optionData.mainVolume;
        sfxVol.value = _optionData.sfxVolume;
        musicVol.value = _optionData.musicVolume;

        fullscreenToggle.isOn = _optionData.fullScreen;
        ToggleFullscreen();
        
        ChangeMasterVolume();
        ChangeSFXVolume();
        ChangeMusicVolume();
    }

    public void SaveOption(ref OptionsData _optionData)
    {
        _optionData.mainVolume = masterVol.value;
        _optionData.sfxVolume = sfxVol.value;
        _optionData.musicVolume = musicVol.value;
        
        _optionData.fullScreen = fullscreenToggle.isOn;
        ToggleFullscreen();
    }
}
