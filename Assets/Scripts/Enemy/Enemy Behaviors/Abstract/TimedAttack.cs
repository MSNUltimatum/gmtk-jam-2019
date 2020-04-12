using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedAttack : Attack
{
    [SerializeField]
    protected float castTime = 0.5f;

    protected override void DoAttack()
    {
        AttackAnimation();
        castTimeLeft = castTime;
        cooldownLeft += castTime;
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        castTimeLeft = Mathf.Max(0, castTimeLeft - Time.deltaTime);
        if (castTimeLeft <= 0)
        {
            castTimeLeft = float.PositiveInfinity;
            CompleteAttack();
        }
    }

    protected void ForceCompleteAttack()
    {
        castTimeLeft = float.PositiveInfinity;
        CompleteAttack();
    }

    protected abstract void AttackAnimation();

    protected abstract void CompleteAttack();

    protected float castTimeLeft = float.PositiveInfinity;
}
