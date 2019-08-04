using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomLighting : MonoBehaviour
{
    private Tilemap tilemap;
    private GameObject Tile;
    private ArenaEnemySpawner arena;
    private float TotalValue = 0;
    private float maxvalue = 0;
    private float CurrentVal;
    static float t = 0.0f;
    static float Light;

    private void Start()
    {
        arena = GetComponent<ArenaEnemySpawner>();
        maxvalue = arena.EnemyCount();
        GameObject TileMap = GameObject.FindGameObjectWithTag("TailMap");
        Tile = TileMap.transform.GetChild(0).gameObject;
        Light = 0.2f + (TotalValue / maxvalue) * 0.8f;
        tilemap = Tile.GetComponent<Tilemap>();
        NewLight(Light);
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
        }
        
        t += Time.deltaTime;
    }
    private void NewLight(float Light)
    {
        tilemap.color = new Color(Light, Light, Light);
    }
}
