using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolHokage : WeaponSkill
{
    private float randomShootingAngle = 0;
    private GameObject bulletPrefab;
    private AudioSource WeaponShot;
    private GameObject Player;
    public PistolHokage()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Name = "PistolHokage";
        Description = "Your second gun";
        ReloadTime = 0.6f;
        bulletPrefab = Resources.Load("Pistol/HeroBullet") as GameObject;
    }
    public override void Shoot(Vector3 mousePos, Vector3 screenPoint)
    {
        Debug.LogWarning("SHOOT");
    }
}
