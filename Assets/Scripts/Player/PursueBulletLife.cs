using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PursueBulletLife : BulletLife
{
    protected override void Move()
    {
        if (targ == null)
        {
            base.Move();
            Targeting();
        }
        else
        {
            if (Vector2.Angle(transform.position, targ.transform.position) % 180 < 10f)
            {
                transform.right = targ.transform.position - transform.position;
                transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);
            }
            else
            {
                if (timeBeforePursue <= 0)
                {
                    transform.right = targ.transform.position - transform.position;
                    transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);
                }
                else
                {
                    transform.right = targ.transform.position - transform.position;
                    timeBeforePursue -= Time.fixedDeltaTime;
                }
            }
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

    private float radius = 2f;
    private float timeBeforePursue = 0.2f;
    private bool isFound = false;
    private float minDistance = float.MaxValue;
    private GameObject targ = null;
}
