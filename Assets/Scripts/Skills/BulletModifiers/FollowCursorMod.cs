using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouseBulletMod", menuName = "ScriptableObject/BulletModifier/MouseBulletMod", order = 1)]
public class FollowCursorMod : BulletModifier
{
    public FollowCursorMod()
    {
        moveTiming = MoveTiming.Final;
        priority = -6; 
    }

    public override void MoveModifier(BulletLife bullet)
    {
        base.MoveModifier(bullet);
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 difference = mousePosition - bullet.transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //rotation_z = Mathf.Clamp(rotation_z, -180, 180);
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }
}
