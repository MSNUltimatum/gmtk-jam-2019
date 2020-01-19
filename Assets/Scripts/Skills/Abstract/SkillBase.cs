﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public abstract class SkillBase : ScriptableObject
{
    [Multiline]
    public string description;
    public Sprite pickupSprite;
    public string itemIndividualName = "";
    protected List<Object> referencesToSave = new List<Object>();

    public string SkillName() => $"{GetType()}:{itemIndividualName}";
    
    protected void OnEnable()
    {
        if (Application.isEditor)
        {
            SkillManager.SaveSkill(SkillName(), this);
        }
    }

    protected void Awake()
    {
        if (Application.isPlaying)
        {
            SkillManager.PrintRegisteredSkills();
            //Debug.Log("Trying to get: " + GetType());
            LoadReferences(SkillManager.LoadSkill(SkillName()));
        }
    }

    /// <summary>
    /// Save to SkillManager -> Registered skills
    /// </summary>
    protected virtual List<Object> SaveReferences() {
        referencesToSave.Add(pickupSprite);
        return referencesToSave;
    }

    /// <summary>
    /// Load from SkillManager -> Registered skills
    /// </summary>
    protected virtual void LoadReferences(List<Object> references) { }

    public abstract void InitializeSkill();

    public abstract void UpdateEffect();
}
