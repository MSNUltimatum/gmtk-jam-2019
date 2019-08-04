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
    private GameObject[] enemyWaves = null;

    static Random random = new Random();

    [SerializeField]
    private Vector2 RoomBounds = new Vector2(15, 10);

    private List<int> randomSequence;
    private int sequenceIndex = 0;

    [SerializeField]
    private EvilDictionary evilDictionary;

    void Start()
    {
        // Get reference for UI current enemy name
        currentEnemy = GetComponent<CurrentEnemy>();
        currentEvilDictionary = evilDictionary;
        randomSequence = GenerateRandom(EnemyCount(), currentEvilDictionary.EvilNames.Length - 1);
    }

    public static List<int> GenerateRandom(int count, int max)
    {
        List<int> result = new List<int>(count);

        // generate count random values.
        HashSet<int> candidates = new HashSet<int>();
        for (int top = max - count; top < max; top++)
        {
            // May strike a duplicate.
            int value = Random.Range(0, top);
            if (candidates.Add(value))
            {
                result.Add(value);
            }
            else
            {
                result.Add(top);
                candidates.Add(top);
            }
        }

        return result;
    }

    public void ChangeTheBoy(GameObject oldBoy)
    {
        boysList.Remove(oldBoy);
        if (boysList.Count != 0)
        {
            var nextBoy = boysList[Random.Range(0, boysList.Count)];
            currentEnemy.SetCurrentEnemy(nextBoy.GetComponentInChildren<TMPro.TextMeshPro>().text, nextBoy);
            nextBoy.GetComponent<MonsterLife>().MakeBoy();
        }
        else
        {
            anyBoy = false;
        }
    }

    private Vector2 RandomBorderSpawnPos()
    {
        var spawnPosition = new Vector2();
        var dice = Random.Range(0, 4);
        // North, South, East, West spawn positions
        switch (dice)
        {
            case 0:
                spawnPosition.y = RoomBounds.y;
                spawnPosition.x = Random.Range(-RoomBounds.x, RoomBounds.x);
                break;
            case 1:
                spawnPosition.y = -RoomBounds.y;
                spawnPosition.x = Random.Range(-RoomBounds.x, RoomBounds.x);
                break;
            case 2:
                spawnPosition.x = RoomBounds.x;
                spawnPosition.y = Random.Range(-RoomBounds.y, RoomBounds.y);
                break;
            case 3:
                spawnPosition.x = RoomBounds.x;
                spawnPosition.y = Random.Range(-RoomBounds.y, RoomBounds.y);
                break;
        }
        return spawnPosition;
    }

    void SpawnMonsters(int waveNum)
    {
        var enemyWave = Instantiate(enemyWaves[waveNum], transform.position, Quaternion.identity);

        int enemiesInWave = enemyWave.transform.childCount;
        EnemiesCount += enemiesInWave;
        // Первый в случайной последовательности будет первым активным врагом в сцене
        for (int i = 0; i < enemiesInWave; i++)
        {
            var enemy = enemyWave.transform.GetChild(i).gameObject;
            if (i == 0)
            {
                if (!anyBoy)
                {
                    anyBoy = true;
                    currentEnemy.SetCurrentEnemy(currentEvilDictionary.EvilNames[randomSequence[sequenceIndex]], enemy);
                    enemy.GetComponent<MonsterLife>().MakeBoy();
                }
            }
            enemy.GetComponentInChildren<TMPro.TextMeshPro>().text = currentEvilDictionary.EvilNames[randomSequence[sequenceIndex]];
            boysList.Add(enemy);
            // Установить случайную позицию персонажам?
            //enemy.transform.position =
            //    new Vector2(Random.Range(-RoomBounds.x, RoomBounds.x),
            //    Random.Range(-RoomBounds.y, RoomBounds.y));
            enemy.transform.position = RandomBorderSpawnPos();
            sequenceIndex++;
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
    public int EnemyCount()
    {
        EnemiesCount = 0;
        foreach (var e in enemyWaves)
        {
            EnemiesCount += e.transform.childCount;
        }
        return EnemiesCount;
    }

    private int EnemiesCount = 0;
    private bool anyBoy = false;
    private int spawnIndex = 0;
    private EvilDictionary currentEvilDictionary;
    private Queue<string> enemyOrder;
    private CurrentEnemy currentEnemy;
    private List<GameObject> boysList = new List<GameObject>();
}
