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
    public Slider masterVolumeSlider;
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
    public AudioMixer myMasterMixer;        

    [Header("Other")]
    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = EventSystem.current;
    }

    private void Start()
    {      
        DontDestroyOnLoad(this);
        SaveData.Instance.Load();        
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

    public void SetMasterVolume(float sliderValue)
    {        
        myMasterMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);        
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
