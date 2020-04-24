using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RicochetMovement : EnemyBehavior
{
    private enum RotateOption
    {
        Randomly,
        TowardsCharacter,
        None
    }

    [SerializeField] private RotateOption rotateOption = RotateOption.Randomly;
    [SerializeField] private float blockTimeAfterHit = 0.75f;

    private void Start()
    {
        switch (rotateOption)
        {
            case RotateOption.Randomly:
                RotateRandomlyAtStart();
                break;
            case RotateOption.TowardsCharacter:
                RotateInstantlyTowardsTarget();
                break;
            default:
                break;
        }
    }

    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();
        steering.linear = transform.up;
        steering.linear *= agent.maxAccel;

        return steering;
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        block -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (block <= 0 && coll.gameObject.tag == "Environment")
        {
            var hits = Physics2D.CircleCastAll(transform.position, 0.3f, transform.up, 2);
            Debug.DrawRay(transform.position, transform.up, Color.green, 1);
            hits = (from t in hits
                    where t.transform.gameObject.tag == "Environment"
                    select t).ToArray();
            if (hits.Length != 0)
            {
                block = blockTimeAfterHit;
                Vector2 direction = Vector2.Reflect(transform.up, hits[0].normal);
                if (direction.magnitude > 0.0f)
                {
                    float rot = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                    GetComponent<AIAgent>().orientation = rot;
                }
            }
        }
    }

    private float block = 0f;
}
