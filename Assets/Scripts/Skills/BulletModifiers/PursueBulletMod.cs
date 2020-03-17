using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "PursueBulletMod", menuName = "ScriptableObject/BulletModifier/PursueBulletMod", order = 1)]
public class PursueBulletMod : BulletModifier
{
    [SerializeField]
    private float factorRotationSpeed = 4f;

    [SerializeField]
    private float pursueBulletAngle = 360f;

    [SerializeField]
    private float radius = 8f;

    public override void MoveModifier(BulletLife bullet)
    {
        base.MoveModifier(bullet);
        if (monsterTargetGameObj == null)
        {
            Targeting(bullet);
        }
        else
        {
            RotateToTarget(monsterTargetGameObj, bullet);
        }
    }

    private void Targeting(BulletLife bullet)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(bullet.transform.position, radius);
        var enemys = (from t in collider2Ds
                      where t.transform.gameObject.tag == "Enemy"
                      select t).ToArray();
        foreach (var i in enemys)
        {
            float dis = Vector2.Distance(bullet.transform.position, i.transform.position);
            if (dis < minDistance)
            {
                minDistance = dis;
                monsterTargetGameObj = i.gameObject;
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

    private void RotateToTarget(GameObject monsterTarget, BulletLife bullet)
    {
        var targetPos = monsterTarget.transform.position;
        var offset = new Vector2(targetPos.x - bullet.transform.position.x, targetPos.y - bullet.transform.position.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var currentAngle = bullet.transform.rotation.eulerAngles.z;
        var difference = angle180fix(angle - currentAngle);
        if (Mathf.Abs(difference) < Mathf.Abs(pursueBulletAngle))
        {
            bullet.transform.rotation = Quaternion.Euler(0, 0, currentAngle + difference * factorRotationSpeed * Time.deltaTime);
        }
    }

    private float minDistance = float.MaxValue;
    private GameObject monsterTargetGameObj = null;
}
