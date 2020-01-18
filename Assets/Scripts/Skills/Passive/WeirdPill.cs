using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "ScriptableObject/PassiveSkill/SpeedBoost", order = 1)]
public class WeirdPill : PassiveSkill
{
    private CharacterMovement character;

    protected WeirdPill()
    {
        name = "SpeedAura";
        description = "Yellow strange pill";
    }

    public override void InitializeSkill()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        character.speed *= 2;
    }
}
