using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class GunfireAnimator : MonoBehaviour
{
    

    new private Light2D light = null;
    
    void Start()
    {
        light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightenTimeLeft > 0)
        {
            light.pointLightInnerRadius = Mathf.Min(0.05f * lightenPower, light.pointLightInnerRadius + 0.05f * lightenSpeed * Time.deltaTime);
            light.pointLightOuterRadius = Mathf.Min(lightenPower, light.pointLightOuterRadius + lightenSpeed * Time.deltaTime);
            lightenTimeLeft = Mathf.Max(0, lightenTimeLeft - Time.deltaTime);
        }
        else
        {
            light.pointLightInnerRadius = Mathf.Max(0, light.pointLightInnerRadius - 0.1f * fadeSpeed * Time.deltaTime);
            light.pointLightOuterRadius = Mathf.Max(0, light.pointLightOuterRadius - fadeSpeed * Time.deltaTime);
        }
    }

    public void LightenUp(float time, float maxPower = 1)
    {
        lightenTimeLeft = time;
        lightenSpeed = 500 * (maxPower / 10);
        lightenPower = 1 * maxPower;
        fadeSpeed = lightenSpeed / 4;
    }

    private float lightenPower = 0;
    private float lightenSpeed = 0;
    private float lightenTimeLeft = 0;
    private float fadeSpeed = 0;
}
