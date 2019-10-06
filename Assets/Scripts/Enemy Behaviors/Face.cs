using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    public override void Awake()
    {
        base.Awake();
        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
    }

    public override EnemySteering GetSteering()
    {
        Vector2 direction = targetAux.transform.position - transform.position;
        if (direction.magnitude > 0.0f)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.y);
            targetOrientation *= Mathf.Rad2Deg;
            target.GetComponent<Agent>().orientation = targetOrientation;
        }
        
        return base.GetSteering();
    }

    private void OnDestroy()
    {
        Destroy(target);
    }

    protected GameObject targetAux;
}
