using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : MonoBehaviour
{
    public static Sound[] sounds;
    public Sound[] soundsToRegister;
    public static AudioManager instance;
    public static Dictionary<string, Vector2> Clips = new Dictionary<string, Vector2>();
    void Awake()
    {
        sounds = soundsToRegister;
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Play("MainTheme");
    }

    public static float GetVolume(string name, float volume)
    {
        if (Clips.ContainsKey(name))
        {
            float ltp = Clips[name].x;
            volume -= 0.1f*(Clips[name].y + Time.time - ltp);   
            //volume -= Time.time - ltp;
            volume = Mathf.Clamp(volume, Clips[name].y/3, Clips[name].y);
            Clips[name] = new Vector2(Time.time,Clips[name].y);
        }
        else
        {
            Clips.Add(name, new Vector2(Time.time, volume));
        } 
        return volume;
    }
    public static void Play(string name, AudioSource source)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = source;
        s.source.clip = s.clip;
        s.source.volume = GetVolume(name,s.volume);
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
