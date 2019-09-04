using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IQ7000DictionarySkills : MonoBehaviour
{
    public static Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private void Start()
    {
        WeirdPill Pill = new WeirdPill();
        skills.Add("SpeedAura", Pill);
        GhostMode ghost = new GhostMode();
        skills.Add("GhostMode", ghost);
    }

    public static SkillBase GetValue(string Name)
    {
        try
        {
            var Val = skills[Name];
            return Val;
        }
        catch
        {
            return null;
        }
    }
}
