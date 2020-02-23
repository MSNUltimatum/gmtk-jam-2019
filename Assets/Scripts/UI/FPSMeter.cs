using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSMeter : MonoBehaviour
{
    private Text text;
    private float fpsSum = 0;
    private uint calculations = 0;
    private float fpsTimer;
    
    void Start()
    {
        text = GetComponent<Text>();    
    }

    void Update()
    {
        fpsTimer += Time.deltaTime;
        if (fpsTimer >= 1)
        {
            fpsTimer = 0;
            calculations++;
            var currentFPS = 1 / Time.deltaTime;
            fpsSum += currentFPS;
            text.text = $"FPS: {(currentFPS).ToString("0.00")}. Average: { (fpsSum / calculations).ToString("0.00") }";
        }
        
    }
}
