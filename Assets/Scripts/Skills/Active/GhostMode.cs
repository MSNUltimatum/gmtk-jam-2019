using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "ScriptableObject/ActiveSkill/Ghost", order = 1)]
public class GhostMode : ActiveSkill
{
    public override void FillSkillInformation()
    {
        description = "Does nothing";
        cooldownDuration = 5f;
        activeDuration = 3f;
    }

    public override void ActivateSkill()
    {
        Debug.Log("Ghost");
    }
}
