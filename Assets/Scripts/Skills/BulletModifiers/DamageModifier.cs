using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseDamageMod", menuName = "ScriptableObject/BulletModifier/IncreaseDamageMod", order = 1)]
public class DamageModifier : BulletModifier
{
    [SerializeField] private float damageMultiplier = 0.1f;

    public override void SpawnModifier(BulletLife bullet)
    {
        base.SpawnModifier(bullet);
        bullet.AddToDamageMultiplier(damageMultiplier);
    }
}
