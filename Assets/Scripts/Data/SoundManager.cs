using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip gameMusic;
    public AudioClip playMusic;
    public AudioClip objectHighlight;
    public AudioClip objectSelect;
    public AudioClip objectTask;
    public AudioClip binggo;
    public AudioClip winGame, loseGame;
    public AudioClip spell0, spell1,  spell2, spell3;
    public AudioClip star1, star2, star3;

    public AudioClip explosion;
    public AudioClip click;
    public AudioClip pop;
    public AudioClip rocket, rocketHit;

    public AudioMixer audioMixer;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của SoundManager tồn tại
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioMixer.SetFloat("MusicVolume", DataManager.Instance.playerData.isMusicOn ? 0 : -80);
        audioMixer.SetFloat("SoundVolume", DataManager.Instance.playerData.isSoundOn ? 0 : -80);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }

    public void Vibration()
    {   
        if(DataManager.Instance.playerData.isVibrationOn) Handheld.Vibrate();
        
    }

}
