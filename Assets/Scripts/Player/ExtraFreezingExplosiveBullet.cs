using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraFreezingExplosiveBullet : ExplosiveBulletLife
{
    protected override void Wave(AIAgent enemy, float thrust)
    {
        var fr = enemy.gameObject.GetComponent<FreezingMonsters>();
        if (!fr)
        {
            enemy.gameObject.AddComponent<FreezingMonsters>();
        }
        else
        {
            fr.Reboot();
        }
    }

    public override void DestroyBullet()
    {
        FindMonsters(gameObject.GetComponent<Collider2D>());
        base.DestroyBullet();
    }


}
