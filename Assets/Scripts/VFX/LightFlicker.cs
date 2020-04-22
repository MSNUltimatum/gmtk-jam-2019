using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private Vector2 FlickerValueRange = new Vector2(-0.02f, 0.02f);
    [SerializeField]
    private Vector2 FlickerPeriodRange = new Vector2(0.25f, 0.25f);
    [SerializeField]
    private float SinFactor = 0.3f;
    [SerializeField]
    private float sinSpeed = 1f;
    [SerializeField]
    bool lightFlicker = true;
    [SerializeField]
    bool spriteFlicker = false;

    // Start is called before the first frame update
    void Awake()
    {
        light = GetComponentInChildren<Light2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        if (light)
        {
            lightStartingIntensity = light.intensity;
        }
        if (sprite)
        {
            alphaStartingIntensity = sprite.color.a;
        }
        timeToFlicker = 0;
        flickerValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lightFlicker)
        {
            light.intensity = lightStartingIntensity + Mathf.Sin(Time.time * sinSpeed) * SinFactor + flickerValue;
        }
        if (spriteFlicker)
        {
            var newColor = sprite.color;
            newColor.a = alphaStartingIntensity + Mathf.Sin(Time.time * sinSpeed) * SinFactor + flickerValue;
            sprite.color = newColor;
        }

        timeToFlicker -= Time.deltaTime;
        if (timeToFlicker <= 0)
        {
            flickerValue = Random.Range(FlickerValueRange.x, FlickerValueRange.y);
            timeToFlicker = Random.Range(FlickerPeriodRange.x, FlickerPeriodRange.y);
        }
    }

    new private Light2D light;
    private SpriteRenderer sprite;
    private float lightStartingIntensity;
    private float alphaStartingIntensity;
    private float timeToFlicker;
    private float flickerValue;
}
