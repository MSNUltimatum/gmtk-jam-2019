using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkills : MonoBehaviour
{
    public List<SkillBase> skills = new List<SkillBase>();
    private void Start()
    {
        PlayerPrefs.SetString("SkillString", "SpeedAura ");
        if(PlayerPrefs.HasKey("SkillString"))
        {
            string AllSkills = PlayerPrefs.GetString("SkillString");
            string[] skillsStr = AllSkills.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in skillsStr)
            {
                skills.Add(IQ7000DictionarySkills.GetValue(i));
            }
            foreach (var i in skills)
            {
                if(i is PassiveSkill)
                    i.ActiAcquireSkill();  
            }
        }

    }
}
