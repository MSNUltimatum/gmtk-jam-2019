using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModifier : ScriptableObject
{
    public int priority = 0;

    public float modifierTime = float.PositiveInfinity;

    public void UpdateMod()
    {
        modifierTime -= Time.deltaTime;
        if (modifierTime < 0) Destroy(this);
    }

    public virtual void ModifierUpdate(BulletLife bullet) { }

    public virtual void HitEnemyModifier(BulletLife bullet, Collider2D coll) { }

    public virtual void DamageEnemyModifier(BulletLife bullet, MonsterLife enemy) { }

    public virtual void HitEnvironmentModifier(BulletLife bullet, Collider2D coll) { }

    public virtual void SpawnModifier(BulletLife bullet) { }

    public virtual void KillModifier(BulletLife bullet, MonsterLife enemy) { }

    public enum MoveTiming { Preparation, Final }
    public MoveTiming moveTiming = MoveTiming.Preparation;
    public virtual void MoveModifier(BulletLife bullet) { }

    // WIP SEGMENT
    public enum BulletTypes
    {
        Bullet
    }

    public bool BulletAvailability() { return true; }

    public BulletTypes[] supportedBulletTypes = new BulletTypes[1] { BulletTypes.Bullet };
}
