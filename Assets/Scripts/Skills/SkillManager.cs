using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    public EquippedWeapon equippedWeapon = null;
    
    [Serializable]
    private class EquippedActiveSkill
    {
        public ActiveSkill skill;
        public float cooldown;
        public float activeTimeLeft;

        public EquippedActiveSkill(ActiveSkill skill)
        {
            this.skill = skill;
            cooldown = 0;
            activeTimeLeft = 0;
        }
    }

    [Serializable]
    public class EquippedWeapon
    {
        public WeaponSkill logic;
        public int ammoLeft;
        public float reloadTimeLeft;
        public int weaponIndex;
        public AudioClip[] attackSound;

        public EquippedWeapon(WeaponSkill weapon, int weaponIndex)
        {
            this.logic = weapon;
            ammoLeft = weapon.ammoMagazine;
            reloadTimeLeft = 0;
            this.weaponIndex = weaponIndex;
            attackSound = weapon.attackSound;
        }
    }

    private void Start()
    {
        InitializeSkills();
        attackManager = GetComponent<CharacterShooting>();
        attackManager.LoadNewWeapon(equippedWeapon, 0);
    }

    private void InitializeSkills()
    {
        var weaponIndex = 0;
        foreach (var s in skills)
        {
            if (s is ActiveSkill)
            {
                activeSkills.Add(new EquippedActiveSkill(s as ActiveSkill));
            }
            else if (s is WeaponSkill)
            {
                equippedWeapons.Add(new EquippedWeapon(s as WeaponSkill, weaponIndex));
                weaponIndex++;
            }
            s.InitializeSkill();
        }
        equippedWeapon = equippedWeapons[0];
    }

    private List<KeyCode> keys = new List<KeyCode>() {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0
    };

    private void Update()
    {
        // Check for a key pressed for active skill
        for (int i = 0; i < activeSkills.Count; i++)
        {
            if (Input.GetKeyDown(keys[i]) && activeSkills.Count >= i && activeSkills[i].cooldown <= 0f)
            {
                activeSkills[i].skill.ActivateSkill();
                activeSkills[i].activeTimeLeft = activeSkills[i].skill.activeDuration;
                activeSkills[i].cooldown = activeSkills[i].skill.cooldownDuration;
            }
        }

        // Update effect, cooldown and active time left for active skill
        for (int i = 0; i < activeSkills.Count; i++)
        {
            activeSkills[i].cooldown = Mathf.Max(0, activeSkills[i].cooldown - Time.deltaTime);

            if (activeSkills[i].activeTimeLeft > 0)
            {
                activeSkills[i].skill.UpdateEffect();
                activeSkills[i].activeTimeLeft = Mathf.Max(0, activeSkills[i].activeTimeLeft - Time.deltaTime);
                if (activeSkills[i].activeTimeLeft <= 0)
                {
                    activeSkills[i].skill.EndOfSkill();
                }
            }
        }

        // Switch weapon
        if (Input.GetKeyDown(rotateWeaponLeft))
        {
            var newWeaponIndex = (equippedWeapon.weaponIndex + equippedWeapons.Count - 1) % equippedWeapons.Count;
            equippedWeapon = equippedWeapons[newWeaponIndex];
            attackManager.LoadNewWeapon(equippedWeapon, equippedWeapon.logic.timeBetweenAttacks);
        }
        else if (Input.GetKeyDown(rotateWeaponRight))
        {
            var newWeaponIndex = (equippedWeapon.weaponIndex + 1) % equippedWeapons.Count;
            equippedWeapon = equippedWeapons[newWeaponIndex];
            attackManager.LoadNewWeapon(equippedWeapon, equippedWeapon.logic.timeBetweenAttacks);
        }

        // Update reload time of all weapons & call update
        foreach (var weapon in equippedWeapons)
        {
            if (weapon.reloadTimeLeft != 0)
            {
                weapon.reloadTimeLeft = Mathf.Max(0, weapon.reloadTimeLeft - Time.deltaTime);
                if (weapon.reloadTimeLeft == 0)
                {
                    weapon.ammoLeft = weapon.logic.ammoMagazine;
                }
            }
            weapon.logic.UpdateEffect();
        }
        equippedWeapon.logic.UpdateEquippedEffect();

        // Update effect of passive skills
        foreach (var s in skills)
        {
            if (s is PassiveSkill)
            {
                s.UpdateEffect();
            }
        }
    }

    public List<SkillBase> skills = new List<SkillBase>();

    private List<EquippedActiveSkill> activeSkills = new List<EquippedActiveSkill>();

    private List<EquippedWeapon> equippedWeapons = new List<EquippedWeapon>();
    private KeyCode rotateWeaponLeft = KeyCode.Q;
    private KeyCode rotateWeaponRight = KeyCode.E;
    private CharacterShooting attackManager;
}
