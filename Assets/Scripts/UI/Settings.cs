using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private Slider SFXSlider = null;
    [SerializeField]
    private Slider musicSlider = null;
    [SerializeField]
    private GameObject settingsScreen = null;
    [SerializeField]
    private GameObject titleScreen = null;

    void Awake()
    {
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        }
        else
        {
            SFXSlider.value = 0.5f;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicSlider.value = 0.5f;
        }
    }

    public void SFXchange() { // called from UI slider
        PlayerPrefs.SetFloat("SoundVolume", SFXSlider.value); // for save/load
        AudioManager.SetVolumeSFX(SFXSlider.value); // for instant change on audioSorce and update vars in AudioManager
    }

    public void MusicChange() { // same for music
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        AudioManager.SetVolumeMusic(musicSlider.value);
    }

    public void ReturnToMenu() {
        titleScreen.SetActive(true);
        settingsScreen.SetActive(false);
    }

}
