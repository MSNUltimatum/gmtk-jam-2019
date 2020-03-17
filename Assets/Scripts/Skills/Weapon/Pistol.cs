using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : ShootingWeapon
{
    protected Pistol()
    {
        description = "Your first gun";
        reloadTime = 1f;
        ammoMagazine = 6;
        timeBetweenAttacks = 0.5f;
    }

}
