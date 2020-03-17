using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CircularStrikeOnDeathSkill", menuName = "ScriptableObject/PassiveSkill/CircularStrikeOnDeath", order = 1)]
public class CircularStrikeOnDeathSkill : PassiveSkill
{
    [SerializeField]
    private CircularStrikeOnDeathMod circularStrikeMod = null;
    public override void InitializeSkill()
    {
        base.InitializeSkill();
        SkillManager.temporaryBulletMods.Add(circularStrikeMod);
    }
}
