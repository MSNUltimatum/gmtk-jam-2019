using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private static float userPrefSound = 0.5f;
    private static float userPrefMusic = 0.5f;

    // Name -> (time since last sound, maximum value)
    public static Dictionary<string, Vector2> Clips = new Dictionary<string, Vector2>();

    private const float lowestSoundValue = 0.3f;

    [SerializeField]
    private AudioSource SourceMusic = null; // duplicate of static for inspector

    [SerializeField]
    AudioClip[] musicList = null;
    [SerializeField]
    private bool restartMusicOnLoad = false;

    void Awake()
    {
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            userPrefSound = PlayerPrefs.GetFloat("SoundVolume");
        }
        else
        {
            userPrefSound = 0.5f;
            PlayerPrefs.SetFloat("SoundVolume", 0.5f);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            userPrefMusic = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            userPrefMusic = 0.5f;
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        }

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audioSourceSFX = GetComponent<AudioSource>();
        audioSourceMusic = SourceMusic; // from inspector to static

        if (audioSourceMusic != null)
        {
            SetVolumeMusic(userPrefMusic);
            MusicCheck();
        }
    }

    private Scene lastFrameScene;

    private void Update()
    {
        Scene newScene = SceneManager.GetActiveScene();
        if (lastFrameScene != newScene) { MusicCheck(); } // if scene changed
        lastFrameScene = newScene;
    }

    void MusicCheck() 
    { // checking if music is correct and changing it if needed        
        int expectedMusicIndex = 0; // index for array musicList, 0 is for no music
        String sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainMenu") { expectedMusicIndex = 2; } //logic for music selection 
        else if (sceneName.Contains("boss")) { expectedMusicIndex = 0; }
        else { expectedMusicIndex = 1; }

        if (expectedMusicIndex == 0){
            audioSourceMusic.Stop();
        } else if (!audioSourceMusic.isPlaying || audioSourceMusic.clip != musicList[expectedMusicIndex])
        {
            audioSourceMusic.Stop();
            audioSourceMusic.clip = musicList[expectedMusicIndex];
            audioSourceMusic.Play();
        } else if (restartMusicOnLoad)
        {
            audioSourceMusic.Stop();
            audioSourceMusic.Play();
        }
    }

    public static float GetVolume(string name, float volume)
    {
        if (Clips.ContainsKey(name))
        {
            float ltp = Clips[name].x;
            volume = Mathf.Lerp(Clips[name].y / 5, Clips[name].y, Time.time - ltp); //sound suppression for multiple identical effects in short time
            Clips[name] = new Vector2(Time.time, Clips[name].y);
        }
        else
        {
            Clips.Add(name, new Vector2(Time.time, volume));
        }
        return volume * userPrefSound;
    }

    public static void Play(string name, AudioSource source)
    {
#if UNITY_WEBGL
        source.volume = GetVolume(name, source.volume / 3);
#else
        source.volume = GetVolume(name, source.volume);
#endif
        if (CharacterLife.isDeath == true)
        {
            source.volume = source.volume / 2;
        }
        source.Play();
    }

    public static void Pause(string name, AudioSource source)
    {
        if (source != null)
        {
            source.Pause();
        }
    }

    public static bool isPlaying(string name, AudioSource source)
    {
        if (source.isPlaying)
            return true;
        else
            return false;
    }

    public static void SetVolumeSFX(float value)
    {
        userPrefSound = value;
#if UNITY_WEBGL
        audioSourceSFX.volume = userPrefSound / 3f;
#else
        audioSourceSFX.volume = userPrefSound;
#endif
    }

    public static void SetVolumeMusic(float value)
    {
        userPrefMusic = value;
#if UNITY_WEBGL
        audioSourceMusic.volume = userPrefMusic / 3f;
#else
        audioSourceMusic.volume = userPrefMusic;
#endif
    }

    public static void PlayMusic(AudioSource sorce)// for externall audio sorce with music volume, like on boss
    {
        sorce.volume = userPrefMusic;
        sorce.Play();
    }

    public static void PauseMusic(AudioSource sorce)
    { 
        sorce.Pause();
    }

    private static AudioSource audioSourceSFX;
    private static AudioSource audioSourceMusic;
}
