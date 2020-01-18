using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    [Multiline]
    public string description;
    public Sprite pickupSprite;

    public void SaveSkill()
    {
        if (PlayerPrefs.HasKey("SkillString"))
        {
            string skills = PlayerPrefs.GetString("SkillString");
            skills += name;
            skills += " ";
            PlayerPrefs.SetString("SkillString", skills);
        }
        else
        {
            string skills = name;
            skills += " ";
            PlayerPrefs.SetString("SkillString", skills);
        }
    }

    public abstract void InitializeSkill();

    public abstract void UpdateEffect();
}
