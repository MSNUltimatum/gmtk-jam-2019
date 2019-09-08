using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolData : WeaponData
{
    public PistolData()
    {
        bullet = Resources.Load("HeroBullet") as GameObject;
    }
}
