using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveSpeedSkill", menuName = "ScriptableObject/ActiveSkill/ActiveSpeedSkill", order = 1)]
public class ActiveSpeedSkill : ActiveSkill
{
    private CharacterMovement character;

    public override void FillSkillInformation()
    {
        name = "ActiveSpeedSkill";
        description = "Yoo could ";
        cooldownDuration = 5f;
        activeDuration = 3f;
    }

    public override void InitializeSkill()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    public override void ActivateSkill()
    {
        character.speed *= 2;
    }

    public override void EndOfSkill()
    {
        character.speed /= 2;
    }
}
