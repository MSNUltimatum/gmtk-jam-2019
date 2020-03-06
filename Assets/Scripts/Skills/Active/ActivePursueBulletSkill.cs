using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActivePursueBulletSkill", menuName = "ScriptableObject/ActiveSkill/ActivePursueBulletSkill", order = 1)]
public class ActivePursueBulletSkill : ActiveSkill
{
    public PursueBulletMod bulletMod;
    protected ActivePursueBulletSkill()
    {
        description = "Bang";
        cooldownDuration = 5f;
        activeDuration = 3f;
    }

    public override void ActivateSkill()
    {
        SkillManager.temporaryBulletMods.Add(bulletMod);
    }

    public override void EndOfSkill()
    {
        ReturnNormalBullets();
    }

    private void ReturnNormalBullets()
    {
        if (SkillManager.temporaryBulletMods.Contains(bulletMod))
        {
            SkillManager.temporaryBulletMods.Remove(bulletMod);
        }
    }
}
