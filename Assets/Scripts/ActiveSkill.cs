using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : SkillBase
{
    public float CoolDown;

    public override void ActiAcquireSkill()
    {
        ActiveResult();
    }
    public virtual void ActiveResult()
    {

    }

}
