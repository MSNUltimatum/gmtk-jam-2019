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

    private void Start()
    {
        arena = GetComponent<ArenaEnemySpawner>();
        maxvalue = arena.EnemyCount();
        GameObject TileMap = GameObject.FindGameObjectWithTag("TailMap");
        Tile = TileMap.transform.GetChild(0).gameObject;
        tilemap = Tile.GetComponent<Tilemap>();
    }

    public void Lighten(float val)
    {
        TotalValue = TotalValue + val;
        NewLight();
    }

    private void NewLight()
    {
        float Light = 0.2f + (TotalValue / maxvalue) * 0.8f;
        tilemap.color = new Color(Light, Light, Light);
    }
}
