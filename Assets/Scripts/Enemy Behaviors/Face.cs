using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : EnemyBehavior
{
    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();

        Vector2 direction = target.transform.position - transform.position;
        float targetOrientation = Mathf.Atan2(direction.x, direction.y);
        targetOrientation *= Mathf.Rad2Deg;
        agent.orientation = targetOrientation;
        
        return steering;
    }
}
