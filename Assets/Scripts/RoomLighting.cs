using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.LWRP;

public class RoomLighting : MonoBehaviour
{
    // Swamp = enemy spawner VFX
    [SerializeField]
    private Material swampMatPrefab = null;
    [SerializeField]
    private GameObject swampPrefab = null;


    private void Start()
    {
        sceneLight = GetComponentInChildren<Light2D>();

        var arena = GetComponent<ArenaEnemySpawner>();
        maxvalue = arena.EnemyCount();
        RecalculateLight();
        NewLight();

        SetSwampMaterial();
    }

    /// <summary>
    /// The function changes the "light" parameter that
    /// is later used to calculate scene lighting as
    /// a current to maximum percentage
    /// </summary>
    /// <param name="val">Value to add to light</param>
    public void AddToLight(float val)
    {
        TotalValue = TotalValue + val;
        RecalculateLight();
        t = 0.0f;
    }

    private void RecalculateLight()
    {
        Light = 0.1f + Mathf.Pow((TotalValue / maxvalue) * 0.9f, 1.3f);
    }

    private void Update()
    {
        if (t < 0.7f)
        {
            if (EXPERIMENTAL)
            {
                CurrentVal = Mathf.Lerp(sceneLight.color.g, Light, t);

            }

            NewLight();
            NewSwampLight();
        }
        
        t += Time.deltaTime;
    }

    bool EXPERIMENTAL = true;

    private void NewLight()
    {
        if (EXPERIMENTAL)
        {
            sceneLight.color = new Color(Light, Light, Light);
        }
        else
        {
            tilemap.color = new Color(Light, Light, Light);
        }
    }

    /// <summary>
    /// Swamp initialization
    /// </summary>
    private void SetSwampMaterial()
    {
        swampMat = new Material(swampMatPrefab);
        swampInstance = Instantiate(swampPrefab);
        var sprites = swampInstance.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            sprite.sharedMaterial = swampMat;
        }
    }
    
    private void NewSwampLight()
    {
        var alpha = 1 - Light;
        var color = swampMat.color;
        color.a = alpha;
        swampMat.color = color;
    }

    private GameObject swampInstance;
    private Material swampMat;

    // Tilemap
    private Tilemap tilemap;
    private Light2D sceneLight;

    private float TotalValue = 0;
    private float maxvalue = 0;
    private float CurrentVal;
    static float t = 0.0f;
    static float Light;
}
