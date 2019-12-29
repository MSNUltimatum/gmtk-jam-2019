using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VignetteShaderTimeInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var image = GetComponent<Image>();
        image.color = Color.black;
        image.material.SetFloat("_TimeSpawn", Time.time);
    }
}
