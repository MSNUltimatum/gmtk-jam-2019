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
    [SerializeField, Tooltip("Leave empty if not needed")]
    private GameObject swampPrefab = null;

    [SerializeField]
    bool StandartLightIncrease = true;
    [SerializeField]
    private float maxvalue = 0;
    public float DefaultLight = 0.13f;

    private void Start()
    {
        var arena = GetComponent<ArenaEnemySpawner>();
        if (arena.labirintMode)
        {
            sceneLight =  Labirint.instance.GetComponentInChildren<Light2D>();
        }
        else
        {
            sceneLight = GetComponentInChildren<Light2D>();
        }
        Light = DefaultLight;
        if (arena && StandartLightIncrease)
        {
            maxvalue = arena.EnemyCount();
            RecalculateLight();
        }
        NewLight(Light);

        if (swampPrefab)
        {
            SetSwampMaterial();
        }
        

        MonsterLife.OnEnemyDead.AddListener(AddOneToLight);
    }

    /// <summary>
    /// The function changes the "light" parameter that
    /// is later used to calculate scene lighting as
    /// a current to maximum percentage
    /// </summary>
    /// <param name="val">Value to add to light</param>
    public void AddToLight(float val, bool automatic = true)
    {
        if (automatic != StandartLightIncrease) return;
        TotalValue = TotalValue + val;
        RecalculateLight();
        t = 0.0f;
    }

    private void AddOneToLight()
    {
        AddToLight(1);
    }

    public void SetMaxValue (float val)
    {
        if (val > 0)
        {
            maxvalue = val;
            RecalculateLight();
            t = 0.0f;
        }
    }

    public float GetCurVal()
    {
        return CurrentVal;
    }

    private void RecalculateLight()
    {
        Light = DefaultLight + Mathf.Pow(Mathf.Clamp01(TotalValue / maxvalue), 1.7f) * (1 - DefaultLight);
       // Debug.Log(Light);
    }

    private void Update()
    {
        if (t < 0.7f)
        {
            if (EXPERIMENTAL)
            {
                CurrentVal = Mathf.Lerp(sceneLight.color.g, Light, t);

            }

            NewLight(CurrentVal);
            if (swampPrefab)
            {
                NewSwampLight();
            }
        }
        
        t += Time.deltaTime;
    }

    bool EXPERIMENTAL = true;

    private void NewLight(float light)
    {
        if (EXPERIMENTAL)
        {
            sceneLight.color = new Color(light, light, light);
        }
    }

    /// <summary>
    /// Swamp initialization
    /// </summary>
    private void SetSwampMaterial()
    {
        swampMat = new Material(swampMatPrefab);
        swampInstance = Instantiate(swampPrefab);
        if (Labirint.instance != null) { // in labirint mode, to delete with parent
            swampInstance.transform.parent = transform;
            swampInstance.transform.localPosition = Vector3.zero;
        }
        var sprites = swampInstance.GetComponentsInChildren<SpriteRenderer>();
        foreach (var sprite in sprites)
        {
            sprite.sharedMaterial = swampMat;
        }
        var emitters = swampInstance.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (var emitter in emitters)
        {
            emitter.sharedMaterial = swampMat;
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
    
    private Light2D sceneLight;

    private float TotalValue = 0;
    private float CurrentVal;
    float t = 0.0f;
    static float Light;
    
}
