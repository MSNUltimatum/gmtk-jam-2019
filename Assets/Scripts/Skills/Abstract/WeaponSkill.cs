using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkill : SkillBase
{
    protected AudioSource[] WeaponAttack;
    public float reloadTime;
    public float timeBetweenAttacks;
    public int ammoMagazine;
    //public int bulletsLeft;

    //public abstract void Shoot(Vector3 mousePos, Vector3 screenPoint);

    public override void InitializeSkill() { }

    public override void UpdateEffect() { }
    public virtual void UpdateEquippedEffect() { }

    public virtual void Attack(Vector3 mousePos, Vector3 screenPoint) { }
}
