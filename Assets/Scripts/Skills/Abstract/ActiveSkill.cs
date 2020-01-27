using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill : SkillBase
{
    public float activeDuration;
    public float cooldownDuration;

    public override void InitializeSkill() { }

    public abstract void ActivateSkill();

    public override void UpdateEffect() { }

    public virtual void EndOfSkill() { }
}
