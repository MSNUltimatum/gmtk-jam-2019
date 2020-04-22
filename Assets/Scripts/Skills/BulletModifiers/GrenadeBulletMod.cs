using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeBulletMod", menuName = "ScriptableObject/BulletModifier/GrenadeBulletMod", order = 1)]
public class GrenadeBulletMod : ExplosiveBulletMod
{ 

    private float maxSpeed;

    public override void SpawnModifier(BulletLife bullet)
    {
        base.SpawnModifier(bullet);
        maxSpeed = bullet.speed;
    }

    public override void ModifierUpdate(BulletLife bullet)
    {
        bullet.speed = (1 - Mathf.Pow(1 - (bullet.TTDLeft / bullet.timeToDestruction), 3)) * maxSpeed;
        base.ModifierUpdate(bullet);
    }

    public override void DestroyModifier(BulletLife bullet)
    {
        base.DestroyModifier(bullet);
        ModEffect(bullet);
    }

    private void ModEffect(BulletLife bulletLife)
    {
        var monsters = FindMonstersNearby(bulletLife);
        ExplosiveWave(monsters, bulletLife);
    }

    protected Collider2D[] FindMonstersNearby(BulletLife bullet)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll( bullet.GetComponent<Collider2D>()
                                                             . ClosestPoint(bullet.transform.position), explosionRadius);
        var enemys = (from t in collider2Ds
                      where t.transform.gameObject.tag == "Enemy"
                      select t).ToArray();
        return enemys;
    }
}
