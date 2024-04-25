using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Interactables")]
    public GameObject playButton;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Toggle fullscreenToggle;
    public GameObject[] mainMenuButtons;

    [Header("Fields")]
    public GameObject nameInputField;

    [Header("Menus")]
    public GameObject nameInputMenu;
    public GameObject settingsMenu;
    public GameObject audioSettingsMenu;
    public GameObject displaySettingsMenu;

    [Header("Mixers")]
    public AudioMixer myMasterMixer;        

    [Header("Other")]
    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = EventSystem.current;
    }

    private void Start()
    {      
       SaveData.Instance.Load();        
    }

    public void Play()
    {
        nameInputMenu.SetActive(true);
        playButton.SetActive(false);

        //For Controller Compatibility
        _eventSystem.SetSelectedGameObject(nameInputField);
    }

    public void SetMasterVolume(float sliderValue)
    {        
        myMasterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);        
    }

    public void SettingsMenu()
    {
        foreach (GameObject button in mainMenuButtons)
        {
            button.SetActive(false);
        }

        settingsMenu.SetActive(true);
    }

    public void AudioSettings()
    {
        settingsMenu.SetActive(false);
        audioSettingsMenu.SetActive(true);
    }

    public void DisplaySettings()
    {
        settingsMenu.SetActive(false);
        displaySettingsMenu.SetActive(true);
    }

    public void Return()
    {
        audioSettingsMenu?.SetActive(false);
        displaySettingsMenu?.SetActive(false);
        settingsMenu.SetActive(true);
        SaveData.Instance.Save();
    }

    public void ToggleFullscreen(bool toggle)
    {
        Screen.fullScreen = toggle;

    }
}
