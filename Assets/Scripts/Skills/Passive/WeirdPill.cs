using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeirdPill : PassiveSkill
{
    private CharacterMovement character;

    public override void FillSkillInformation()
    {
        name = "SpeedAura";
        description = "Yellow strange pill";
    }

    public override void InitializeSkill()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();

    }

    public override void UpdateEffect()
    {
        character.speed *= 2;
    }
}
