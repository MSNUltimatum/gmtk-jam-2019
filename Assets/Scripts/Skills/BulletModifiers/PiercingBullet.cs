using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiercingMod", menuName = "ScriptableObject/BulletModifier/PiercingMod", order = 1)]
public class PiercingBullet : BulletModifier
{
    public override void SpawnModifier(BulletLife bullet)
    {
        base.SpawnModifier(bullet);
        bullet.piercing = true;
    }
}
