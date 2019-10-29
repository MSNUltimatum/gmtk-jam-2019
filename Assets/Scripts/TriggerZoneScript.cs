using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneScript : MonoBehaviour
{
    [SerializeField]
    private GameObject WavePref;
    [SerializeField]
    private GameObject currentSpanZone;
    private ArenaEnemySpawner spawner;
    private bool firstSpawn = true;
    private SpriteRenderer sprite;

    private void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        Color color1 = sprite.color;
        color1.a = 0f;
        sprite.color = color1;
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        spawner = manager.GetComponent<ArenaEnemySpawner> ();
    }

    private void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" && WavePref != null && firstSpawn)
        {
            for (int i = 0;i < WavePref.transform.childCount; i++)
            {
                if (currentSpanZone && WavePref)
                    spawner.SpawnIntoSpawnZone(WavePref.transform.GetChild(i).gameObject, currentSpanZone);
            }
            firstSpawn = false;
        }
    }
}
