using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneScript : MonoBehaviour
{
    [SerializeField]
    private GameObject WavePref;
    private ArenaEnemySpawner spawner;

    private void Start ()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        spawner = manager.GetComponent<ArenaEnemySpawner> ();
    }

    private void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" && WavePref != null)
        {
            for (int i = 0;i < WavePref.transform.childCount; i++)
            {
                spawner.SpawnIntoSpawnZone(WavePref.transform.GetChild(i).gameObject);
            }
        }
    }
}
