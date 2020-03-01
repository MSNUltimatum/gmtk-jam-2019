using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhasingMod", menuName = "ScriptableObject/BulletModifier/PhasingMod", order = 1)]
public class PhasingBullet : BulletModifier
{
    public override void SpawnModifier(BulletLife bullet)
    {
        base.SpawnModifier(bullet);
        bullet.phasing = true;
    }
}
