using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : SkillBase
{
    public float CoolDown;
    public bool isInstantSkill;
    public float ActionTime;
    public bool isActive;

    public override void ActiAcquireSkill()
    {
        ActiveResult();
    }
    public virtual void ActiveResult()
    {  

    }

    public virtual void EndOfSkill()
    {

    }
}
