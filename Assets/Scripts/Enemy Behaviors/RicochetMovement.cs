using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetMovement : EnemyBehavior
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        transform.LookAt((target.transform.position - gameObject.transform.position).normalized, Vector3.forward);
    }

    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();
        steering.linear = transform.up;
        steering.linear *= agent.maxAccel;

        return steering;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Environment")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1);
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
