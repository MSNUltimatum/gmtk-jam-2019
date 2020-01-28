﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PursueBulletLife : BulletLife
{
    [SerializeField]
    private float PursueBulletAngle = 360f;
    protected override void Move()
    {
        if (targ == null)
        {
            base.Move();
            Targeting();
        }
        else
        {
            transform.Translate(Vector2.right * Speed * Time.deltaTime, Space.Self);
            RotateToTarget(targ);
            /*if (Vector2.Angle(transform.position, targ.transform.position) % 180 < 10f)
            {
                transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);
            }
            else
            {
                if (timeBeforePursue <= 0)
                {
                    transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);
                }
                else
                {
                    timeBeforePursue -= Time.fixedDeltaTime;
                }
            }*/
            TTDLeft -= Time.fixedDeltaTime;
        }
    }

    private void Targeting()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius);
        var enemys = (from t in collider2Ds
                      where t.transform.gameObject.tag == "Enemy"
                      select t).ToArray();
        foreach(var i in enemys)
        {
            float dis = Vector2.Distance(transform.position, i.transform.position);
            if (dis < minDistance)
            {
                minDistance = dis;
                targ = i.gameObject;
            }
        }
    }

    private float angle180fix(float angle)
    {
        if (angle > 180)
        {
            return -360 + angle;
        }
        else if (angle < -180)
        {
            return 360 + angle;
        }
        else return angle;
    }

    private void RotateToTarget(GameObject target)
    {
        var targetPos = target.transform.position;
        var offset = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var currentAngle = gameObject.transform.rotation.eulerAngles.z;
        var difference = angle180fix(angle - currentAngle);
        if (Mathf.Abs(difference) < Mathf.Abs(PursueBulletAngle))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, currentAngle + difference * 4 * Time.deltaTime);
        }
    }

    private float radius = 8f;
    private float timeBeforePursue = 0.5f;
    private bool isFound = false;
    private float minDistance = float.MaxValue;
    private GameObject targ = null;
}
