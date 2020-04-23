using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            var hits = Physics2D.CircleCastAll(transform.position, 1, transform.up, 2);
            hits = (from t in hits
                    where t.transform.gameObject.tag == "Environment"
                    select t).ToArray();
            if (hits.Length != 0)
            {
                Vector2 direction = Vector2.Reflect(transform.up, hits[0].normal);
                if (direction.magnitude > 0.0f)
                {
                    float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                    GetComponent<AIAgent>().orientation = rot;
                }
            }
        }
    }
}
