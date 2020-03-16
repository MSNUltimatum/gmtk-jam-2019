using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class ArenaEnemySpawner : MonoBehaviour
{
    public Vector2 RoomBounds = new Vector2(15, 10);

    [SerializeField]
    private float timeToEachSpawn = 5;
    [SerializeField]
    private float timeToNextSpawn = 0;

    [SerializeField]
    protected GameObject[] enemyWaves = null;



    public SpawnZoneScript SpawnZone = null;

    [SerializeField]
    protected bool AllowEarlySpawns = true;

    [SerializeField]
    private bool isInfSpawn = false;

    public static int boysCount = 0;

    public bool labirintMode = false;

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Room") != null)  // for room in labirint variation
            labirintMode = true;
        roomLighting = GetComponent<RoomLighting>();
        scenesController = GetComponent<RelodScene>();
        isPointVictory = scenesController.isPointVictory;

        if (!labirintMode)
        {
            GameObject SpawnSquare = GameObject.FindGameObjectWithTag("SpawnZone");
            if (SpawnSquare)
            {
                SpawnZone = SpawnSquare.GetComponent<SpawnZoneScript>();
            }
        }
    }

    void Start()
    {
        InitializeFields();
    }

    private void InitializeFields()
    {
        // Listens for "Enemy dead" event to lower the number of enemies on screen
        MonsterLife.OnEnemyDead.AddListener(LowerBoysCount);

        boysList = new List<GameObject>();
        boysCount = 0;
        enemiesCount = baseEnemyCount();
    }

    //public static void ChangeTheBoy(GameObject oldBoy)
    //{
    //    if (scenesController)
    //    {
    //        scenesController.UpdateScore(1);
    //    }
    //    roomLighting.AddToLight(1);

    //    boysList.Remove(oldBoy);
    //    if (boysList.Count != 0)
    //    {
    //        var nextBoy = boysList[Random.Range(0, boysList.Count)];
    //        CurrentEnemyUI.SetCurrentEnemy(nextBoy);
    //        nextBoy.GetComponent<MonsterLife>().MakeBoy();
    //        currentBoy = nextBoy;
    //    }
    //    else
    //    {
    //        anyBoy = false;
    //    }
    //}

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
                spawnPosition.x = -RoomBounds.x;
                spawnPosition.y = Random.Range(-RoomBounds.y, RoomBounds.y);
                break;
        }
        if (labirintMode)
        {
            spawnPosition += (Vector2)gameObject.transform.position; // shift to room position
        }
        return spawnPosition;
    }

    protected void SetMonsterPosition(GameObject enemy)
    {
        enemy.transform.position = RandomBorderSpawnPos();
    }

    private void SpawnMonsters(int waveNum)
    {
        var enemyWave = Instantiate(enemyWaves[waveNum], transform.position, Quaternion.identity);

        int enemiesInWave = enemyWave.transform.childCount;

        for (int i = 0; i < enemiesInWave; i++)
        {
            var enemy = enemyWave.transform.GetChild(i).gameObject;
            var behaviours = enemy.GetComponentsInChildren<EnemyBehavior>();
            foreach (var behaviour in behaviours)
            {
                behaviour.Activate();
            }
            // Set random enemy name from the dictionary
            //enemy.GetComponentInChildren<TMPro.TextMeshPro>().text = currentEvilDictionary[sequenceIndex];

            boysList.Add(enemy);
            boysCount++;

            if (!SpawnZone)
            {
                SetMonsterPosition(enemy);
            }
            else
            {
                enemy.transform.position = SpawnZone.SpawnPosition();
            }

            sequenceIndex++;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Pause.Paused) return;

        EnemySpawnUpdate();
        if (Labirint.instance == null)
            if (RelodScene.isVictory)
            {
                KillThemAll();
            }
    }

    public void KillThemAll()
    {
        var iToKill = 0;
        while (iToKill < boysList.Count)
        {            
            if (boysList[iToKill] != null) 
            {
                boysList[iToKill].GetComponent<MonsterLife>().Damage(null, 999, ignoreInvulurability: true);
            }
            iToKill++;
        }
    }

    protected void EnemySpawnUpdate()
    {
        timeToNextSpawn -= Time.deltaTime;
        if ((timeToNextSpawn < 0 || boysCount == 0 && AllowEarlySpawns) && spawnIndex < enemyWaves.GetLength(0) &&
            sequenceIndex < scenesController.monsterAdditionLimit + enemiesCount)
        {
            timeToNextSpawn = timeToEachSpawn;
            SpawnMonsters(spawnIndex);
            spawnIndex++;

            if (spawnIndex >= enemyWaves.GetLength(0))
            {
                if (isInfSpawn)
                {
                    spawnIndex = 0;
                }
            }
        }
    }

    public int EnemyCount()
    {
        return isPointVictory ? scenesController.pointsToVictory : baseEnemyCount();
    }

    public int baseEnemyCount()
    {
        enemiesCount = 0;
        foreach (var e in enemyWaves)
        {
            enemiesCount += e.transform.childCount;
        }
        return enemiesCount;
    }

    /// <summary>
    /// Spawn the monster with random name
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    public GameObject SpawnMonster(GameObject monster)
    {
        var enemy = Instantiate(monster, transform.position, Quaternion.identity);
        boysList.Add(enemy);

        if (!SpawnZone)
        {
            SetMonsterPosition(enemy);
        }
        else
        {
            enemy.transform.position = SpawnZone.SpawnPosition();
        }

        sequenceIndex++;
        return enemy;
    }

    public GameObject SpawnMonster(GameObject monster, string name)
    {
        var createdMonster = SpawnMonster(monster);
        createdMonster.GetComponentInChildren<TMPro.TextMeshPro>().text = name;
        return createdMonster;
    }

    private void LowerBoysCount()
    {
        boysCount--;
    }

    private int enemiesCount = 0;
    private int sequenceIndex = 0;
    protected int spawnIndex = 0;

    protected static GameObject currentBoy;

    public static List<GameObject> boysList = new List<GameObject>();

    private static RoomLighting roomLighting;
    private static RelodScene scenesController;

    private bool isPointVictory = false;
    public bool IsInfSpawn { get { return isInfSpawn; } }
}
