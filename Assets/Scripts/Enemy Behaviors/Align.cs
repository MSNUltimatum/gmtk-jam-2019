using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : EnemyBehavior
{
    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();
        /*
        float targetOrientation =
            target.GetComponent<Agent>().orientation;
        */
        return steering;
    }
}
