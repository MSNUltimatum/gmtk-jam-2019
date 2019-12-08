using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetMovement : EnemyBehavior
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        transform.forward = (target.transform.position - gameObject.transform.position).normalized;
    }

    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();
        steering.linear = transform.forward * agent.maxAccel;

        return steering;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Environment")
        {
            var direction = transform.forward;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction,
                    float.PositiveInfinity, LayerMask.GetMask("Default"));
            if (hit)
            {
                direction = Vector2.Reflect(direction, hit.normal);
            }
        }
    }
}
