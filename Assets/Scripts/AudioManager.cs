using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public static Dictionary<string, Vector2> Clips;
    void Awake()
    {
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
        Play("MainTheme");
    }

    public float GetVolume(string name, float volume)
    {
        float timestamp = Time.time;
        Clips = new Dictionary<string, Vector2>();       
        //Clips.Add("MainTheme", new Vector2(0.8f, 0.9f));
        if (!Clips.ContainsKey(name))
        {
            Clips.Add(name, new Vector2(timestamp,volume));
        }
        volume -= timestamp - Time.time;
        return volume;
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = gameObject.AddComponent<AudioSource>();
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
