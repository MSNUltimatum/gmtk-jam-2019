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

    [SerializeField] bool StandartLightIncrease = true;
    [SerializeField] private float maxvalue = 0;
    [SerializeField] private float roomClearedLight = 0.8f;
    public float DefaultLight = 0.13f;

    private void Start()
    {
        var arena = GetComponent<ArenaEnemySpawner>();
        if (Labirint.instance != null)
        {
            sceneLight = Labirint.instance.GetComponentInChildren<Light2D>();
            if (swampPrefab)
            {
                SetSwampMaterial();
            }
        }
        else
        {
            sceneLight = GetComponentInChildren<Light2D>();
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
        Light = Mathf.Lerp(DefaultLight, roomClearedLight, Mathf.Pow(Mathf.Clamp01(TotalValue / maxvalue), 1.7f));
       // Debug.Log(Light);
    }

    private void Update()
    {
        if (t < 0.7f)
        {
            if (EXPERIMENTAL)
            {
                CurrentVal = Mathf.Lerp(sceneLight.intensity, Light, t);

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
            sceneLight.intensity = light;
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

    public void LabirintRoomEnterDark(int enemyCount)
    {
        t = 0.0f;
        Light = DefaultLight;
        maxvalue = enemyCount;
        TotalValue = 0;
        RecalculateLight();
    }

    public void LabirintRoomEnterBright() 
    {
        Light = roomClearedLight;
    }

    public void LabirintRoomAddLight()
    {
        AddToLight(1);
    }

}
