using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomLighting : MonoBehaviour
{
    // Tilemap
    private Tilemap tilemap;
    private GameObject Tile;
    private ArenaEnemySpawner arena;
    private float TotalValue = 0;
    private float maxvalue = 0;
    private float CurrentVal;
    static float t = 0.0f;
    static float Light;

    // Swamp - enemy spawner
    [SerializeField]
    private Material swampMatPrefab = null;
    [SerializeField]
    private GameObject swampPrefab = null;

    private void Start()
    {
        arena = GetComponent<ArenaEnemySpawner>();
        maxvalue = arena.EnemyCount();
        GameObject TileMap = GameObject.FindGameObjectWithTag("TailMap");
        Tile = TileMap.transform.GetChild(0).gameObject;
        Light = 0.2f + (TotalValue / maxvalue) * 0.8f;
        tilemap = Tile.GetComponent<Tilemap>();
        NewLight(Light);

        SetSwampMaterial();
    }

    public void Lighten(float val)
    {
        TotalValue = TotalValue + val;
        Light = 0.2f + (TotalValue / maxvalue) * 0.8f;
        t = 0.0f;
    }

    private void Update()
    {
        if (t < 0.7f)
        {
            CurrentVal = Mathf.Lerp(tilemap.color.g, Light, t);
            NewLight(CurrentVal);
            NewSwampLight(CurrentVal);
        }
        
        t += Time.deltaTime;
    }
    private void NewLight(float Light)
    {
        tilemap.color = new Color(Light, Light, Light);
    }

    // Swamp
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

    // Light is between 0.2 and 1
    private void NewSwampLight(float Light)
    {
        var alpha = 1 - Light;
        var color = swampMat.color;
        color.a = alpha;
        swampMat.color = color;
    }

    private GameObject swampInstance;
    private Material swampMat;
}
