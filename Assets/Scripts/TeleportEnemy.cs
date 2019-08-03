using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemy : EnemyMovement
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
        Teleport();
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

    private void Teleport()
    {
        if(CoolDownBefore == 0)
        {
            CoolDownBefore = Rand();
            float Xpos = Random.Range(Player.transform.position.x, transform.position.x);
            float YPos = Random.Range(Player.transform.position.y, transform.position.y);
            var vect = new Vector2(Player.transform.position.x - Xpos, Player.transform.position.y - YPos);
            vect.Normalize();
            vect *= 3f;
            transform.position = vect;
        }
    }

    private float Rand()
    {
        return Random.Range(7.5f, 9f);
    }
}
