using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDrop : Container
{
    private MonsterLife monsterLife;

    protected override void Awake()
    {
        monsterLife = GetComponent<MonsterLife>();
        monsterLife.hpChangedEvent.AddListener(DeathCheck);
        base.Awake(); 
    }

    private void DeathCheck() {
        if (monsterLife.HP <= 0) { 
            //animation?
            Open();
        }
    }
}
