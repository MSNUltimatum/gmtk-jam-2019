using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bypass : Align
{
    [SerializeField]
    protected float maxRaycastDistance = 2.5f;

    public override EnemySteering GetSteering()
    {
        Vector2 direction = target.transform.position - transform.position;
        var hits = RaycastHits(direction.normalized);
        hits = (from t in hits
                where t.transform.gameObject.tag == "Environment" || t.transform.gameObject.tag == "Player"
                select t).ToArray();
        if (hits.Length == 0 || hits[0].transform.gameObject.tag == "Player")
        {
            if (direction.magnitude > 0.0f)
            {
                float targetOrientation = Mathf.Atan2(direction.x, direction.y);
                targetOrientation *= Mathf.Rad2Deg;
                base.targetOrientation = targetOrientation;
            }
            return base.GetSteering();
        }
        else
        {
            if (direction.magnitude > 0.0f)
            {
                var selectItems = from t in hits
                                  where t.transform.gameObject.tag == "Environment"
                                  select t;
                var environments = selectItems.ToArray();
                if (environments.Length > 0)
                {
                    float targetOrientation = Mathf.Atan2(direction.x, direction.y);
                    targetOrientation *= Mathf.Rad2Deg;
                    Vector2 direct = Vector2.Reflect(direction.normalized, environments[0].normal);
                    float rot = Mathf.Atan2(direct.x, direct.y) * Mathf.Rad2Deg;
                    targetOrientation += rot * Time.deltaTime * 53;
                    Debug.Log(targetOrientation);
                    targetOrientation %= 360.0f;
                    if (targetOrientation < 0.0f)
                    {
                        targetOrientation += 360.0f;
                    }
                    base.targetOrientation = targetOrientation;
                }
            }
            return base.GetSteering();
        }
    }

    private RaycastHit2D[] RaycastHits(Vector2 direction)
    {
        return Physics2D.RaycastAll(transform.position, direction, maxRaycastDistance);
    }
}
