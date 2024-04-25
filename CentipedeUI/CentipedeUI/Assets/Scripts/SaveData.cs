using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance { get; private set; }

    public MainMenu menuManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else Instance = this;
    }

    struct gameSettings
    {
        public float masterVolume;
        public float musicVolume;
        public bool isFullscreen;
    }

    public void Save()
    {
        gameSettings gs = new gameSettings();

        //Volume
        gs.masterVolume = menuManager.masterVolumeSlider.value;
        gs.musicVolume = menuManager.musicVolumeSlider.value;

        //Display
        gs.isFullscreen = menuManager.fullscreenToggle.isOn;

        //Post-Process


        //Accessability


        string m_Value = JsonUtility.ToJson(gs);
        string filePath = Application.persistentDataPath + "/Centipede_Settings.json";
        Debug.Log(filePath);
        Debug.Log(m_Value);
        System.IO.File.WriteAllText(filePath, m_Value);
    }

    public void Load()
    {
        string filePath = Application.persistentDataPath + "/Centipede_Settings.json";
        string m_Value = System.IO.File.ReadAllText(filePath);
        Debug.Log(m_Value);
        gameSettings gs = JsonUtility.FromJson<gameSettings>(m_Value);

        //Volume
        menuManager.masterVolumeSlider.value = gs.masterVolume;
        menuManager.musicVolumeSlider.value = gs.musicVolume;

        //Display
        menuManager.fullscreenToggle.isOn = gs.isFullscreen;

        //Post-Process


        //Accessability

    }

    public bool CheckDir(string dir)
    {
        if (Directory.Exists(dir))
        {
            return true;
        }
        else return false;
    }
}
