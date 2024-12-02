using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    public Toggle  toggleMusic;
    public Toggle  toggleSound;
    public Toggle  toggleVibration;

    public AudioMixer audioMixer;


    void Start()
    {
        SetStateToggle();
    }


    private void SetStateToggle()
    {
        if(toggleMusic.isOn == DataManager.Instance.playerData.isMusicOn)
        {
            toggleMusic.isOn = !DataManager.Instance.playerData.isMusicOn;
            ToggleMusic();
        }
        
        if(toggleSound.isOn == DataManager.Instance.playerData.isSoundOn)
        {
            toggleSound.isOn = !DataManager.Instance.playerData.isSoundOn;
            ToggleSound();
        }

        if(toggleVibration.isOn == DataManager.Instance.playerData.isVibrationOn)
        {
            toggleVibration.isOn = !DataManager.Instance.playerData.isVibrationOn;
            ToggleVibration();
        }
    }

    public void ToggleMusic()
    {
        DataManager.Instance.playerData.isMusicOn = !DataManager.Instance.playerData.isMusicOn;
        audioMixer.SetFloat("MusicVolume", DataManager.Instance.playerData.isMusicOn ? 0 : -80);
    }

    public void ToggleSound()
    {
        DataManager.Instance.playerData.isSoundOn = !DataManager.Instance.playerData.isSoundOn;
        audioMixer.SetFloat("SoundVolume", DataManager.Instance.playerData.isSoundOn ? 0 : -80);
    }

    public void ToggleVibration()
    {
        DataManager.Instance.playerData.isVibrationOn = !DataManager.Instance.playerData.isVibrationOn;
    }
}
