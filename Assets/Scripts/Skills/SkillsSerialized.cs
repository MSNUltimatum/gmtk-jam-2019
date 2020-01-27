using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillsRecord
{
    private const int activeSkillsMaxCount = 10;
    private const int passiveSkillsMaxCount = 255;
    private const int weaponMaxCount = 10;

    public string[] activeSkills;
    public string[] passiveSkills;
    public string[] weapons;
    int i, j, k;

    public SkillsRecord(List<SkillBase> skills)
    {
        activeSkills = new string[activeSkillsMaxCount];
        passiveSkills = new string[passiveSkillsMaxCount];
        weapons = new string[weaponMaxCount];

        // indices for arrays ^^^
        i = j = k = 0;
        
        foreach (var skill in skills)
        {
            if (skill is PassiveSkill)
            {
                passiveSkills[i] = skill.SkillName();
                i++;
            }
            else if (skill is ActiveSkill)
            {
                activeSkills[j] = skill.SkillName();
                j++;
            }
            else
            {
                weapons[k] = skill.SkillName();
                k++;
            }
        }
    }
}
