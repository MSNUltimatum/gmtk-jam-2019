using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// <para>This script is loaded automatically only in editor after all other scripts are loaded</para>
/// Its purpose is to load all Skills into SkillContainer prefab and save it.<para />
/// That prefab can be accessed in game to load skills by their names<para />
/// </summary>
[InitializeOnLoad]
public class SkillAssetLoader
{
    static SkillAssetLoader()
    {
        EditorApplication.update += RunOnce; // Runs only when the scene is fully loaded
    }

    static void RunOnce()
    {
        EditorApplication.update -= RunOnce; // Stop running this script
        RegisterSkills();
    }

    static void RegisterSkills()
    {
        if (!Application.isPlaying)
        {
            Dictionary<string, SkillBase> registeredSkills = new Dictionary<string, SkillBase>();
            var scrObjectsGUID = AssetDatabase.FindAssets("t:SkillBase");
            foreach (var scrObjectGUID in scrObjectsGUID)
            {
                var skillAsset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(scrObjectGUID), typeof(SkillBase)) as SkillBase;
                registeredSkills.Add(skillAsset.SkillName(), skillAsset);
            }
            var skillContainerPath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SkillContainer")[0]);
            var skillContainer = AssetDatabase.LoadAssetAtPath(skillContainerPath, typeof(GameObject)) as GameObject;

            var skillContainerSkills = skillContainer.GetComponent<SkillPullFromDatabase>().LoadSkills();
            
            // We might not need to update prefab there is no difference in information
            if (skillContainerSkills == null || skillContainerSkills.Count == 0 || !skillContainerSkills.OrderBy(kvp => kvp.Key).SequenceEqual(registeredSkills.OrderBy(kvp => kvp.Key)))
            {
                skillContainer.GetComponent<SkillPullFromDatabase>().RegisterSkills(registeredSkills);
                PrefabUtility.SavePrefabAsset(skillContainer, out bool savedSuccess);
                Debug.Log($"Registered skills successfully.? {savedSuccess}");
            }
        }
    }
}
