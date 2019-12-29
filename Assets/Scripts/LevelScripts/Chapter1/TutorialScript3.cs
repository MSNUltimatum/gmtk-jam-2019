using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript3 : ArenaEnemySpawner
{
    private bool firstSpawn = true;
    private bool firstDeath = true;
    public bool isVictoryT = false;
    protected override void Update()
    {
        if (firstSpawn)
        {
            SpawnСertainMonsterWithName(enemyWaves[0].transform.GetChild(2).gameObject, "Reluctance");
            SpawnZone = null;
            SpawnCertainMonsterWithoutName(enemyWaves[0].transform.GetChild(0).gameObject);
            MakeMonsterActive("Reluctance");
            CurrentEnemy.SetCurrentEnemyName("Shoot even if you can't kill it");
            firstSpawn = false;
        }

        if(isVictoryT && firstDeath)
        {
            KillThemAll();
            firstDeath = false;
            CurrentEnemy.SetCurrentEnemyName("");
        }
    }
}