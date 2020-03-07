using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreezingMod", menuName = "ScriptableObject/BulletModifier/FreezingMod", order = 1)]
public class FreezingMod : BulletModifier
{
    public override void DamageEnemyModifier(BulletLife bullet, MonsterLife enemy)
    {
        base.DamageEnemyModifier(bullet, enemy);
        Freeze(enemy);
    }

    [SerializeField]
    private float freezingDuration = 1f;
    private void Freeze(MonsterLife other)
    {
        var fr = other.GetComponent<FreezingMonsters>();
        if (!fr)
        {
            var freezingComp = other.gameObject.AddComponent<FreezingMonsters>();
            freezingComp.MyStart(freezingDuration);
        }
        else
        {
            fr.Reboot();
        }
    }
}
