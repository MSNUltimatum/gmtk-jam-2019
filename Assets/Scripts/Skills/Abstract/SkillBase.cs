using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class SkillBase : ScriptableObject
{
    [Multiline]
    public string description;
    public Sprite pickupSprite;

    public string SkillName() => $"{GetType()}:{this.name.Substring(0, this.name.IndexOf("(Clone)") == -1 ? this.name.Length : (this.name.IndexOf("(Clone)")))}";

    public abstract void InitializeSkill();

    public abstract void UpdateEffect();
}
