using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSkill : SkillBase
{
    public AudioClip[] attackSound;
    public float reloadTime;
    public float timeBetweenAttacks;
    public int ammoMagazine;
    //public int bulletsLeft;

    //public abstract void Shoot(Vector3 mousePos, Vector3 screenPoint);

    public override void InitializeSkill() { }

    public override void UpdateEffect() { }
    public virtual void UpdateEquippedEffect() { }

    public virtual void Attack(CharacterShooting attackManager, Vector3 mousePos, Vector3 screenPoint) { }
    public virtual int AmmoConsumption() { return 1; }
}
