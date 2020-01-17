using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMode : ActiveSkill
{
    public override void FillSkillInformation()
    {
        name = "GhostMode";
        description = "Yoo could walk";
        cooldownDuration = 5f;
        activeDuration = 3f;
    }

    public override void ActivateSkill()
    {
        Debug.Log("Ghost");
    }
}
