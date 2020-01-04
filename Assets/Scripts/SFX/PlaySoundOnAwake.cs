using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnAwake : MonoBehaviour
{
    [SerializeField]
    private bool shouldPlaySound = false;
    
    [Header("Non-default name?")]
    [Tooltip("Can be left blank if there is no need in specific name")]
    [SerializeField]
    private string clipName = "";

    void Awake()
    {
        if (shouldPlaySound)
        {
            var source = GetComponent<AudioSource>();
            var name = clipName == "" ? source.clip.name : clipName;
            AudioManager.Play(name, source);
        }    
    }
}
