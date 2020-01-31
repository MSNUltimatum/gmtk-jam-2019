using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActivePursueBulletSkill", menuName = "ScriptableObject/ActiveSkill/ActivePursueBulletSkill", order = 1)]
public class ActivePursueBulletSkill : ActiveSkill
{
    public GameObject bulletPrefab;
    private SkillManager skillManager;
    protected ActivePursueBulletSkill()
    {
        description = "Bang";
        cooldownDuration = 5f;
        activeDuration = 3f;
    }

    public override void InitializeSkill()
    {
        skillManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SkillManager>();
    }

    public override void ActivateSkill()
    {
        var skillsWeapon = skillManager.skills;
        foreach(var i in skillsWeapon)
        {
            if(i is ShootingWeapon)
            {
                ((ShootingWeapon)i).currentBulletPrefab = bulletPrefab;
            }
        }
    }

    public override void EndOfSkill()
    {
        var skillsWeapon = skillManager.skills;
        foreach (var i in skillsWeapon)
        {
            if (i is ShootingWeapon)
            {
                ((ShootingWeapon)i).currentBulletPrefab = ((ShootingWeapon)i).bulletPrefab;
            }
        }
    }
}
