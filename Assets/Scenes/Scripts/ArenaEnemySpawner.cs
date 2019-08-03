using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemySpawner : MonoBehaviour
{
    [SerializeField]
    private float timeToEachSpawn = 5;
    [SerializeField]
    private float timeToNextSpawn = 0;

    [SerializeField]
    private GameObject[] enemyWaves;
    int spawnIndex = 0;

    void SpawnMonsters(int waveNum)
    {
        var enemyWave = Instantiate(enemyWaves[waveNum]);
        for (int i = 0; i < enemyWave.transform.childCount; i++)
        {
            // Установить случайную позицию, случайные имена
            //GameObject.Instantiate(monster, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextSpawn -= Time.deltaTime;

        if (timeToNextSpawn < 0 && spawnIndex < enemyWaves.GetLength(0))
        {
            timeToNextSpawn = timeToEachSpawn;
            SpawnMonsters(spawnIndex);
            spawnIndex++;

            if (spawnIndex > enemyWaves.GetLength(0)) { }
        }
    }
}
