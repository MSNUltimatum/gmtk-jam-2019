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
            var boyMonster = SpawnMonster(enemyWaves[0].transform.GetChild(2).gameObject, "Reluctance");
            SpawnZone = null;
            SpawnMonster(enemyWaves[0].transform.GetChild(0).gameObject);
            GetComponent<CurrentEnemySelector>().SelectEnemy(boyMonster);
            CurrentEnemyUI.SetCurrentEnemy("Shoot even if you can't kill it");
            firstSpawn = false;
        }

        if(isVictoryT && firstDeath)
        {
            KillThemAll();
            firstDeath = false;
            CurrentEnemyUI.SetCurrentEnemy("");
        }
    }
}