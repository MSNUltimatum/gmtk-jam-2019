using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MawMonsterLife : MonsterLife
{
    public bool Opened = false;

    protected override bool Vulnurable()
    {
        return Opened;
    }
}
