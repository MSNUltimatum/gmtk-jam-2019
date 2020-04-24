using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddModPassiveSkill", menuName = "ScriptableObject/PassiveSkill/AddModPassiveSkill", order = 1)]
public class AddModPassiveSkill : PassiveSkill
{
    [SerializeField]
    private BulletModifier modToAdd = null;
    public override void InitializeSkill()
    {
        base.InitializeSkill();
        SkillManager.temporaryBulletMods.Add(modToAdd);
    }
}
