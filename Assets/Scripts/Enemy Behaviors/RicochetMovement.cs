using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetMovement : EnemyBehavior
{
    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();
        steering.linear = transform.forward;
        steering.linear *= agent.maxAccel;

        return steering;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Environment")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1, LayerMask.GetMask("Default"));
            if (hit)
            {
                print("yosh");
                print(transform.up);
                transform.LookAt(Vector2.Reflect(transform.up, hit.normal), Vector3.forward);
                print(transform.up);
            }
        }
    }
}
