using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreezingMod", menuName = "ScriptableObject/BulletModifier/FreezingMod", order = 1)]
public class FreezingMod : BulletModifier
{
    public override void HitEnemyModifier(BulletLife bullet, Collider2D coll)
    {
        base.HitEnemyModifier(bullet, coll);
        Freeze(coll);
    }

    [SerializeField]
    private float freezingDuration = 1f;
    private void Freeze(Collider2D other)
    {
        var fr = other.gameObject.GetComponent<FreezingMonsters>();
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
