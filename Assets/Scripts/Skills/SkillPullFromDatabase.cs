using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>This is a mediator between Editor and In-game logic. 
/// Contains all registered skills. SkillAssetLoader.cs automatically loads them here.</para>
/// 
/// Two lists are used in order to serialize scripts. 
/// </summary>
public class SkillPullFromDatabase : MonoBehaviour
{
    [SerializeField]
    private List<string> registeredSkillNames = new List<string>();
    [SerializeField]
    private List<SkillBase> registeredSkills = new List<SkillBase>();

    public void RegisterSkills(Dictionary<string, SkillBase> inputRegisteredSkills)
    {
        registeredSkillNames = new List<string>();
        registeredSkills = new List<SkillBase>();
        foreach (var key in inputRegisteredSkills.Keys)
        {
            registeredSkillNames.Add(inputRegisteredSkills[key].SkillName());
            registeredSkills.Add(inputRegisteredSkills[key]);
        }
    }

    public Dictionary<string, SkillBase> LoadSkills()
    {
        var skillToReturn = new Dictionary<string, SkillBase>();
        for (int i = 0; i < registeredSkillNames.Count; i++)
        {
            skillToReturn.Add(registeredSkillNames[i], registeredSkills[i]);
        }
        return skillToReturn;
    }
}
