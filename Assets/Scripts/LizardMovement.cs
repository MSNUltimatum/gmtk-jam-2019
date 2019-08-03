using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardMovement : EnemyMovement
{
    private float CoolDownBefore;

    protected override void Start()
    {
        CoolDownBefore = Rand();
        base.Start();
    }

    protected override void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        base.Update();
    }

    protected override void MoveToward()
    {
        ExtraSpeed();
        base.MoveToward();
    }

    protected override void Rotation()
    {
        base.Rotation();
    }

    private void ExtraSpeed()
    {
        if (CoolDownBefore < 2f)
        {
            EnemySpeed = 3.5f;
        }

        if (CoolDownBefore == 0)
        {
            CoolDownBefore = Rand();
            EnemySpeed = 2f;
        }
    }
    private float Rand()
    {
        return Random.Range(6f, 9f);
    }

}
