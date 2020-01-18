using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "ScriptableObject/Weapon/Pistol", order = 1)]
public class Pistol : ShootingWeapon
{
    public override void FillSkillInformation()
    {
        description = "Your first gun";
        reloadTime = 1f;
        ammoMagazine = 6;
        timeBetweenAttacks = 0.5f;
    }

    public override void InitializeSkill()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
