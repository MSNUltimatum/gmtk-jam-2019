using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActiveMouseBulletDkill", menuName = "ScriptableObject/ActiveSkill/ActiveMouseBulletSkill", order = 1)]
public class ActiveMouseBullet : ActiveSkill
{
    public GameObject bulletPrefab;
    private SkillManager skillManager;
    protected ActiveMouseBullet()
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
        foreach (var i in skillsWeapon)
        {
            if (i is ShootingWeapon)
            {
                ((ShootingWeapon)i).currentBulletPrefab = bulletPrefab;
            }
        }
        ShootingWeapon.shootingEvents.AddListener(ReturnNormalBullets);
    }

    public override void EndOfSkill()
    {
        ReturnNormalBullets();
    }

    private void ReturnNormalBullets()
    {
        var skillsWeapon = skillManager.skills;
        foreach (var i in skillsWeapon)
        {
            if (i is ShootingWeapon)
            {
                ((ShootingWeapon)i).currentBulletPrefab = ((ShootingWeapon)i).bulletPrefab;
            }
        }
        ShootingWeapon.shootingEvents.RemoveListener(ReturnNormalBullets);
    }
}
