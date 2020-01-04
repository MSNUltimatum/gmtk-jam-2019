using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bypass : Align
{
    [SerializeField]
    protected float maxRaycastDistance = 2f;

    public override EnemySteering GetSteering()
    {
        direction = target.transform.position - transform.position;
        var hits = RaycastHits(direction);
        hits = (from t in hits
                where t.transform.gameObject.tag == "Environment" || t.transform.gameObject.tag == "Player"
                select t).ToArray();
        if(hits.Length == 0 || hits[0].transform.gameObject.tag == "Player")
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
            var environment = (from t in hits
                                where t.transform.gameObject.tag == "Environment"
                                select t).ToArray();
            direct = (target.transform.position - transform.position).normalized;
            direct += environment[0].normal * 2;
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), 2 * Time.deltaTime);
            rot.x = 0;
            rot.y = 0;
            targetOrientation += rot.z * Mathf.Rad2Deg * Time.deltaTime;
            base.targetOrientation = targetOrientation;
            return base.GetSteering();
        }
    }

    private RaycastHit2D[] RaycastHits(Vector2 direction1)
    {
        //Debug.DrawLine(transform.position, direction1.normalized);
        Debug.DrawRay(transform.position, direction1.normalized * maxRaycastDistance, Color.green);
        return Physics2D.RaycastAll(transform.position, direction, maxRaycastDistance);
    }

    private bool isWall = false;
    Vector2 direction;
    Vector2 direct;
    RaycastHit2D newTarget = new RaycastHit2D();
    float offset;
}
