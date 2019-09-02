using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMode : ActiveSkill
{

    public GhostMode()
    {
        Name = "GhostMode";
        Description = "Yoo could walk";
        CoolDown = 5f;
    }

    public override void ActiveResult()
    {
        Debug.Log("GhostMode");
    }
}
