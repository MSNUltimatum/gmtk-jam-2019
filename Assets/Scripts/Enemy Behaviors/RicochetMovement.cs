using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetMovement : EnemyBehavior
{
    private void Start()
    {
        RotateInstantlyTowardsTarget();
    }

    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();
        steering.linear = transform.up;
        steering.linear *= agent.maxAccel;

        return steering;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Environment")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 2, LayerMask.GetMask("Default"));
            if (hit)
            {
                Vector2 direction = Vector2.Reflect(transform.up, hit.normal);
                if (direction.magnitude > 0.0f)
                {
                    float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                    print("yosh: " + transform.up + "///" + hit.normal + "///" + direction + "///" + rot);
                    GetComponent<AIAgent>().orientation = rot;
                }
            }
        }
    }
}
