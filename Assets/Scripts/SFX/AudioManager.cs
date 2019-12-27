﻿using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : MonoBehaviour
{
    public static Sound[] sounds;
    public Sound[] soundsToRegister;
    public static AudioManager instance;
    public static float userPrefSound = 0.5f;

    // Name -> (time since last sound, maximum value)
    public static Dictionary<string, Vector2> Clips = new Dictionary<string, Vector2>();

    private const float lowestSoundValue = 0.3f;

    void Awake()
    {
        sounds = soundsToRegister;
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
    }

    public static float GetVolume(string name, float volume)
    {
        if (Clips.ContainsKey(name))
        {
            float ltp = Clips[name].x;
#if UNITY_WEBGL
            volume = Mathf.Lerp(0.02f, Clips[name].y, Mathf.Clamp(Time.time - ltp, 0, 1));
#else
            volume = Mathf.Lerp(lowestSoundValue, Clips[name].y, Mathf.Clamp(Time.time - ltp, 0, 1));
#endif
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

        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = source;
        s.source.clip = s.clip;
        
        s.source.pitch = s.pitch;
        s.source.playOnAwake = s.playOnAwake;
        s.source.loop = s.loop;
        s.source.mute = s.mute;
#if UNITY_WEBGL
        s.source.volume = GetVolume(name, s.volume / 3);
#else
        s.source.volume = GetVolume(name, s.volume);
        s.source.panStereo = s.stereoPan;
        s.source.spatialBlend = s.spatialBlend;
        s.source.reverbZoneMix = s.reverbZoneMix;
        s.source.bypassEffects = s.bypassEffects;
        s.source.bypassReverbZones = s.bypassReverbZones;
        s.source.dopplerLevel = s.dopplerLevel;
        s.source.spread = s.spread;
        s.source.minDistance = s.minDistance;
        s.source.maxDistance = s.maxDistance;
#endif

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (CharacterLife.isDeath == true)
        {
            s.source.volume = s.volume / 2;
        }
        s.source.Play();
    }
    public static void Pause(string name, AudioSource source)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source = source;
            s.source.Pause();
        }
    }
    public static bool isPlaying(string name, AudioSource source)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = source;
        if (s.source.isPlaying)
            return true;
        else
            return false;
    }
}