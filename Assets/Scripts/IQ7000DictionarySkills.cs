using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IQ7000DictionarySkills : MonoBehaviour
{
    public static Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private void Start()
    {
        WeirdPill Pill = new WeirdPill();
        skills.Add("SpeedAura", Pill);
    }

    public static SkillBase GetValue(string Name)
    {
        foreach(KeyValuePair<string, SkillBase> keyValue in skills)
        {
            if (keyValue.Key == Name)
                return keyValue.Value;
        }
        return null;
    }
}
