using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    //done:
    // перенести ArenaSpawner, монстры должны спавнится - done
    // перенести reloadScene, обработка победы - done
    // обработка смерти и перерождения - done
    // убедиться что свет работает - done
    // добавить учет бродячих мобов - done
    // отключать бродячих мобов при спавне комнаты до входа  - done
    // включать бродячих мобов при входе - done
    // не включать мобов если комната уже завершена - done
    // починить гребаный свет еще раз - done

    //todo:
    // перепроверить светлячков - проверил - нихрена не работают
    // собрать несколько волн с новыми монстрами и затестить
    // перепроверить паузу

    //to do or not to do?...
    // spawnZone???? посмотреть можно ли их прикрутить
    // переключатель условия выхода: надо убивать всех мобов, убить количество, открыто при входе
    // проверить inf spawn

    //cleanup:
    // обновить префабы с монстер менеджером
    // скрипты по папкам
    // пересмотреть видимы-невидимые поля в инспекторе

    public Vector2 RoomBounds = new Vector2(15, 10);
    [SerializeField]
    private float timeToEachSpawn = 5;
    [SerializeField]
    private float timeToNextSpawn = 0;
    [SerializeField]
    protected GameObject[] enemyWaves = null;

    public bool spawnAvailable = false;
    public RoomLighting roomLighting;
    public List<GameObject> strayMonsters;
    public List<GameObject> monsterList;

    private Room room;

    [SerializeField]
    protected bool AllowEarlySpawns = true;
    protected int spawnIndex = 0;

    void Awake()
    {
        roomLighting = GetComponent<RoomLighting>();
        strayMonsters = new List<GameObject>();
        if (GetComponent<Room>() != null)
        {
            room = GetComponent<Room>();
            room.monsterManager = this;
        }
        else
            Debug.LogError("MonsterManager can't find room script");

        foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (monster.transform.IsChildOf(transform))
            { 
                strayMonsters.Add(monster);
                monsterList.Add(monster);
                monster.SetActive(false);
                monster.GetComponent<MonsterLife>().monsterManager = this;
            }
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
                spawnPosition.x = -RoomBounds.x;
                spawnPosition.y = Random.Range(-RoomBounds.y, RoomBounds.y);
                break;
        }
        spawnPosition += (Vector2)gameObject.transform.position; // shift to room position
        return spawnPosition;
    }

    protected void SetMonsterPosition(GameObject enemy)
    {
        enemy.transform.position = RandomBorderSpawnPos();
    }


    private void SpawnMonsters(int waveNum)
    {
        var enemyWave = Instantiate(enemyWaves[waveNum], transform.position, Quaternion.identity);
        enemyWave.transform.parent = room.transform; //чтобы удалять лишних вместе с комнатой

        int enemiesInWave = enemyWave.transform.childCount; //может пойти не так если есть чайлды которые не монстры, может добавить проверку...

        for (int i = 0; i < enemiesInWave; i++)
        {
            GameObject enemy = enemyWave.transform.GetChild(i).gameObject;
            var behaviours = enemy.GetComponentsInChildren<EnemyBehavior>();
            // Make enemies move towards player always
            foreach (var behaviour in behaviours)
            {
                if (!(behaviour is Attack))
                {
                    behaviour.Activate();
                    behaviour.timeToLoseAggro = -1; // never stop moving
                }
            }
            // Set random enemy name from the dictionary
            //enemy.GetComponentInChildren<TMPro.TextMeshPro>().text = currentEvilDictionary[sequenceIndex];

            monsterList.Add(enemy);
            enemy.GetComponent<MonsterLife>().monsterManager = this;

            //if (!SpawnZone)
            //{
                SetMonsterPosition(enemy);
            //}
            //else
            //{
            //    enemy.transform.position = SpawnZone.SpawnPosition();
            //}

            //sequenceIndex++;
        }
    }

    public void Death(GameObject monster) {        
        roomLighting.labirintRoomAddLight();
        monsterList.Remove(monster);
        if (strayMonsters.Contains(monster))
            strayMonsters.Remove(monster);
        WinCheck();
    }

    void WinCheck() {
        //сюда возможно условие на режим комнаты
        if (monsterList.Count == 0 && spawnIndex == enemyWaves.GetLength(0)) {
            room.UnlockRoom();
        }
    }

    protected virtual void Update()
    {
        if (Pause.Paused) return;
        if (spawnAvailable)
            EnemySpawnUpdate();
        if (CharacterLife.isDeath)
        {
            GameObject pressFGUI = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).gameObject;
            if (!pressFGUI.activeSelf)
                pressFGUI.SetActive(true);
        }
        //if (Labirint.instance == null)
        //  if (RelodScene.isVictory)
        //{
        //  KillThemAll();
        //}
    }

    public void KillThemAll()
    {
        foreach (GameObject monster in monsterList)
            monster.GetComponent<MonsterLife>().Damage(null, 999, ignoreInvulurability: true);
        monsterList = new List<GameObject>();
        strayMonsters = new List<GameObject>();
    }

    protected void EnemySpawnUpdate()
    {
        timeToNextSpawn -= Time.deltaTime;
        if ((timeToNextSpawn < 0 || monsterList.Count == 0 && AllowEarlySpawns) && spawnIndex < enemyWaves.GetLength(0) 
            //&& sequenceIndex < scenesController.monsterAdditionLimit + enemiesCount
            )
        {
            timeToNextSpawn = timeToEachSpawn;
            SpawnMonsters(spawnIndex);
            spawnIndex++;

            //if (spawnIndex >= enemyWaves.GetLength(0))
            //{
            //    if (isInfSpawn)
            //    {
            //        spawnIndex = 0;
            //    }
            //}
        }
    }

    public int EnemyCount()
    {
        int enemiesCount = 0;
        foreach (var e in enemyWaves)
        {
            enemiesCount += e.transform.childCount;
        }
        enemiesCount += strayMonsters.Count;
        return enemiesCount;
    }

    public void UnfreezeMonsters() {
        spawnAvailable = true;
        foreach (GameObject monster in strayMonsters) {
            monster.SetActive(true);
        }
    }
}
