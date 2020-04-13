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
            Vector2 fireflyPos = Vector2.zero;
            if (Labirint.instance == null)
            {
                fireflyPos = new Vector2(
                    Random.Range(-arena.RoomBounds.x, arena.RoomBounds.x),
                    Random.Range(-arena.RoomBounds.y, arena.RoomBounds.y));
            }
            else
            {
                MonsterManager monsterManager = Labirint.GetCurrentRoom().GetComponent<MonsterManager>();
                if (monsterManager != null)
                {
                    fireflyPos = new Vector2(
                        Random.Range(monsterManager.transform.position.x - monsterManager.RoomBounds.x, monsterManager.transform.position.x + monsterManager.RoomBounds.x),
                        Random.Range(monsterManager.transform.position.y - monsterManager.RoomBounds.y, monsterManager.transform.position.x + monsterManager.RoomBounds.y));
                }
                else {
                    Debug.Log("FireflySpawner cant find current room MonsterManager");
                    fireflyPos = Vector2.zero;
                    // надо придумат как обрабатывать комнаты без арена спавнера и монстер менеджера
                }

            }
            timeToNextSpawn = Random.Range(timeToEachSpawn.x, timeToEachSpawn.y);
            var aFirefly = Instantiate(firefly, fireflyPos, Quaternion.identity);
            Destroy(aFirefly, 10);

        }
    }

    private ArenaEnemySpawner arena;
}
