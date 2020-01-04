using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private static float userPrefSound = 0.5f;

    // Name -> (time since last sound, maximum value)
    public static Dictionary<string, Vector2> Clips = new Dictionary<string, Vector2>();

    private const float lowestSoundValue = 0.3f;

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

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            Play("MainTheme", audioSource);
        }
    }

    public static float GetVolume(string name, float volume)
    {
        if (Clips.ContainsKey(name))
        {
            float ltp = Clips[name].x;
            volume = Mathf.Lerp(Clips[name].y / 5, Clips[name].y, Time.time - ltp);
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
}
