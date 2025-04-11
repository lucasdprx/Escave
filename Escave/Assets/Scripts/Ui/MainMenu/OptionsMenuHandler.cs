using System.Collections.Generic;
using System.Linq;
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
    
    private Resolution[] _resolutions;

    private void Start()
    {
        if (resolutionDropdown != null)
            GetResolution();
    }

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

        //fullscreenToggle.isOn = _optionData.fullScreen;
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
    private Resolution[] GetResolution()
    {
        _resolutions = Screen.resolutions.Select(resolution => 
            new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolution = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
            
            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
            {
                currentResolution = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();
        
        return _resolutions;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ReturnButton()
    {
        gameObject.SetActive(false);
    }
}
