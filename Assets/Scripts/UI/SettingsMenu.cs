using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private AudioMixer _mainMixer;
    [SerializeField] private Slider _volumeSlider;

    Resolution[] _allResolutions;
    bool _isFullScreen;
    int _selectedResolution;
    List<Resolution> _resolutions = new List<Resolution>();

    void Start()
    {
        _allResolutions = Screen.resolutions;

        _isFullScreen = PlayerPrefs.GetInt("FullScreen", Screen.fullScreen ? 1 : 0) == 1;
        _selectedResolution = PlayerPrefs.GetInt("SelectedResolution", -1);

        List<string> resolutionsString = new List<string>();
        string newRes;

        foreach (Resolution resolution in _allResolutions) 
        {
            newRes = resolution.width.ToString() + " x " + resolution.height.ToString();
            if (!resolutionsString.Contains(newRes))
            {
                resolutionsString.Add(newRes);
                _resolutions.Add(resolution);
            }
           
        }

        _resolutionDropdown.AddOptions(resolutionsString);

        if (_selectedResolution == -1)
        {
            Resolution currentResolution = Screen.currentResolution;
            for (int i = 0; i < _resolutions.Count; i++)
            {
                if (_resolutions[i].width == currentResolution.width && _resolutions[i].height == currentResolution.height)
                {
                    _selectedResolution = i;
                    break;
                }
            }
        }

        _resolutionDropdown.value = _selectedResolution;

        _fullScreenToggle.isOn = _isFullScreen;

        float savedVolume = PlayerPrefs.GetFloat("Volume", 0f);
        _volumeSlider.value = savedVolume;
        _mainMixer.SetFloat("MasterVolume", savedVolume);
    }

    public void ChangeResolution()
    {
        _selectedResolution = _resolutionDropdown.value;
        PlayerPrefs.SetInt("SelectedResolution", _selectedResolution);
        Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, _isFullScreen);
    }


    public void ChangeFullScreen()
    {
        _isFullScreen = _fullScreenToggle.isOn;
        PlayerPrefs.SetInt("FullScreen", _isFullScreen ? 1 : 0);
        Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, _isFullScreen);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        _mainMixer.SetFloat("MasterVolume", volume);
    }

    public void ExitSettings()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
