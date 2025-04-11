using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour, IOptionPersistence
{
    public Slider masterVol, sfxVol, musicVol;
    public AudioMixer mainAudioMixer;

    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    [SerializeField] private List<string> resolutions;
    
    string _currentResolution;

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
        
        _currentResolution = _optionData.resolution;

        for (int i = 0; i < resolutions.Count; i++)
        {
            if (String.Equals(resolutions[i], _currentResolution))
            {
                resolutionDropdown.value = i;
            }
        }
        string[] _resolution = _currentResolution.Split('x');
        Screen.SetResolution(int.Parse(_resolution[0]), int.Parse(_resolution[1]), fullscreenToggle.isOn);

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

        _optionData.resolution = _currentResolution;
        
        _optionData.fullScreen = fullscreenToggle.isOn;
        ToggleFullscreen();
    }
    
    public void SetResolution()
    {
        _currentResolution = resolutions[resolutionDropdown.value];
        string[] _resolution = _currentResolution.Split('x');
        Screen.SetResolution(int.Parse(_resolution[0]), int.Parse(_resolution[1]), fullscreenToggle.isOn);
    }
}
