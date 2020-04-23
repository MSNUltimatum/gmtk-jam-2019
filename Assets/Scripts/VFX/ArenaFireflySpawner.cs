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
    private Dictionary<Direction.Side, float> borders = new Dictionary<Direction.Side, float>();

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
            Vector2 fireflyPos = Vector2.zero;            
            GetBorders();
            fireflyPos = new Vector2(
                    Random.Range(borders[Direction.Side.LEFT], borders[Direction.Side.RIGHT]),
                    Random.Range(borders[Direction.Side.DOWN], borders[Direction.Side.UP]));
            timeToNextSpawn = Random.Range(timeToEachSpawn.x, timeToEachSpawn.y);
            var aFirefly = Instantiate(firefly, fireflyPos, Quaternion.identity);
            Destroy(aFirefly, 10);
        }
    }

    void GetBorders() {
        if (Labirint.instance == null)
        {
            if (arena != null)
            {
                borders[Direction.Side.LEFT] = -arena.RoomBounds.x;
                borders[Direction.Side.RIGHT] = arena.RoomBounds.x;
                borders[Direction.Side.UP] = arena.RoomBounds.y;
                borders[Direction.Side.DOWN] = -arena.RoomBounds.y;
            }
            else Debug.LogError("Can't get arena or labirint script");
        }
        else {
            Room room = GetComponent<Room>();
            if (room != null)
            {
                borders = room.GetBordersFromTilemap();
            }
            else Debug.LogError("Can't get room script");
        }
    }

    private ArenaEnemySpawner arena;
}
