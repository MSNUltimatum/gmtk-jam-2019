using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraFreezingExplosiveBullet : ExplosiveBulletLife
{
    protected override void Wave(Rigidbody2D Enemy, float thrust)
    {
        var fr = Enemy.gameObject.GetComponent<FreezingMonsters>();
        if (!fr)
        {
            Enemy.gameObject.AddComponent<FreezingMonsters>();
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
