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

    Resolution[] _allResolutions;
    bool _isFullScreen;
    int _selectedResolution;
    List<Resolution> _resolutions = new List<Resolution>();

    void Start()
    {
        _isFullScreen = true;
        _allResolutions = Screen.resolutions;

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
    }

    public void ChangeResolution()
    {
        _selectedResolution = _resolutionDropdown.value;
        Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, _isFullScreen);
    }


    public void ChangeFullScreen()
    {
        _isFullScreen = _fullScreenToggle.isOn;
        Screen.SetResolution(_resolutions[_selectedResolution].width, _resolutions[_selectedResolution].height, _isFullScreen);
    }

    public void SetVolume(float volume)
    {
        _mainMixer.SetFloat("MasterVolume", volume);
    }

    public void ExitSettings()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
