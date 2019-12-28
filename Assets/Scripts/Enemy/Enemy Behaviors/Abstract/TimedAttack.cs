using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedAttack : Attack
{
    [SerializeField]
    private float castTime = 0.5f;

    protected override void DoAttack()
    {
        AttackAnimation();
        castTimeLeft = castTime;
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        castTimeLeft = Mathf.Max(0, castTimeLeft - Time.deltaTime);
        if (castTimeLeft <= 0)
        {
            castTimeLeft = int.MaxValue;
            CompleteAttack();
        }
    }

    protected abstract void AttackAnimation();

    protected abstract void CompleteAttack();

    private float castTimeLeft = 0;
}
