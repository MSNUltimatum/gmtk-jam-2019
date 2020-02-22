using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDrop : Container
{
    protected override void Awake()
    {
        MonsterLife.OnEnemyDead.AddListener(DeathCheck);
        base.Awake(); 
    }

    private void DeathCheck() {
        if (GetComponent<MonsterLife>().HP <= 0) { 
            //animation?
            Open();
        }
    }
}
