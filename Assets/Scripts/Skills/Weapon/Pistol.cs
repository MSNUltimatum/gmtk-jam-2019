using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : ShootingWeapon
{
    public override void FillSkillInformation()
    {
        name = "Pistol";
        description = "Your first gun";
        reloadTime = 1f;
        ammoMagazine = 6;
        timeBetweenAttacks = 0.5f;
        //bulletPrefab = Resources.Load("Pistol/HeroBullet") as GameObject;
    }

    public override void InitializeSkill()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private GameObject Player;
}
