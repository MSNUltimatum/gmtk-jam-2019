using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaFireflySpawner : MonoBehaviour
{
    [SerializeField]
    private Vector2 timeToEachSpawn = new Vector2(5f, 15f);
    private float timeToNextSpawn;
    [SerializeField]
    private GameObject firefly = null;

    void Start()
    {
        arena = GetComponent<ArenaEnemySpawner>();
        timeToNextSpawn = Random.Range(timeToEachSpawn.x, timeToEachSpawn.y);
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextSpawn -= Time.deltaTime;
        if (timeToNextSpawn < 0)
        {
            var fireflyPos = new Vector2(
                Random.Range(-arena.RoomBounds.x, arena.RoomBounds.x), 
                Random.Range(-arena.RoomBounds.y, arena.RoomBounds.y));
            timeToNextSpawn = Random.Range(timeToEachSpawn.x, timeToEachSpawn.y);
            var aFirefly = Instantiate(firefly, fireflyPos, Quaternion.identity);
            Destroy(aFirefly, 10);

        }
    }

    private ArenaEnemySpawner arena;
}
