using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyMovement
{
    private float CoolDownBefore;
    [SerializeField]
    private float CoolDown = 18f;
    private BoxCollider2D BoxCollider;
    


    protected override void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        CoolDownBefore = CoolDown;
        base.Start();
    }

    protected override void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        GhostMode();
        base.Update();
    }

    protected override void MoveToward()
    {
        base.MoveToward();
    }

    protected override void Rotation()
    {
        base.Rotation();
    }

    private void GhostMode()
    {
        if(CoolDownBefore < 3f)
        {
            BoxCollider.isTrigger = true;
            EnemySpeed = 3.5f;
        }

        if(CoolDownBefore == 0)
        {
            CoolDownBefore = CoolDown;
            BoxCollider.isTrigger = false;
            EnemySpeed = 2f;
        }

    }

}
