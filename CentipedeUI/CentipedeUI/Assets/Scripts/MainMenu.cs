using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Interactables")]
    public GameObject playButton;
    public GameObject[] mainMenuButtons;
    public Image loadingBarFill;
    public Slider SFXVolumeSlider;
    public Slider musicVolumeSlider;
    public Toggle fullscreenToggle;        

    [Header("Fields")]
    public GameObject nameInputField;
    public TMP_InputField nameInputText;

    [Header("Menus")]
    public GameObject nameInputMenu;
    public GameObject settingsMenu;
    public GameObject audioSettingsMenu;
    public GameObject displaySettingsMenu;
    public GameObject loadingScreenMenu;
    public GameObject mainMenu;

    [Header("Mixers")]
    public AudioMixer myMusicMixer;
    public AudioMixer mySFXMixer;

    [Header("Other")]   
    bool isPaused;
    
    private void Start()
    {      
        DontDestroyOnLoad(this);

        string filePath = Application.persistentDataPath + "/Centipede_Settings.json";

        if (!System.IO.File.Exists(filePath))
        {
            Debug.Log("Creating New Save File");
            SaveData.Instance.Save();
            SaveData.Instance.Load();
        }
        else if (System.IO.File.Exists(filePath))
        {
            Debug.Log("Loading Existing Save File");
            SaveData.Instance.Load();
        }
    }

    private void Update()
    {
        int y = SceneManager.GetActiveScene().buildIndex;

        if (Input.GetKeyDown(KeyCode.Escape) && y == 1)
        {
            Pause();
        }
    }

    public void Pause()
    {        
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            settingsMenu.SetActive(true);
        }
        else
        {
            settingsMenu.SetActive(false);
            displaySettingsMenu.SetActive(false);
            audioSettingsMenu.SetActive(false);
            SaveData.Instance.Save();
            Time.timeScale = 1;
            isPaused = false;
        }        
    }

    public void Play()
    {
        nameInputMenu.SetActive(true);
        playButton.SetActive(false);
        SaveData.Instance.Save();
    }    

    public void CancelNameCreation()
    {
        nameInputMenu.SetActive(false);
        playButton.SetActive(true);
    }

    public void ConfirmNameCreation()
    {
        SaveData.Instance.Save();
    }

    public void SetMusicVolume(float sliderValue)
    {        
        myMusicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);        
    }

    public void SetSFXVolume(float sliderValue)
    {
        mySFXMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void AudioSettings()
    {
        audioSettingsMenu.SetActive(true);
    }

    public void DisplaySettings()
    {
        displaySettingsMenu.SetActive(true);
    }    

    public void ToggleFullscreen(bool toggle)
    {
        Screen.fullScreen = toggle;
        SaveData.Instance.Save();
    }

    public void LoadingScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        //loadingScreenMenu.SetActive(true);
        nameInputMenu.SetActive(false);
        mainMenu.SetActive(false);
        audioSettingsMenu.SetActive(false);
        displaySettingsMenu.SetActive(false);
        settingsMenu.SetActive(false);

        yield return new WaitForSeconds(3f);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / .9f);

            loadingBarFill.fillAmount = progressValue;

            yield return null;            
        }
    }
}
