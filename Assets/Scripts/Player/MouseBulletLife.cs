using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBulletLife : BulletLife
{
    void FixedUpdate()
    {
        if (Pause.Paused) return;

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);

        Vector3 difference = mousePosition - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //rotation_z = Mathf.Clamp(rotation_z, -180, 180);
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }

    protected override void Move()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);

        Vector3 difference = mousePosition - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //rotation_z = Mathf.Clamp(rotation_z, -180, 180);
        transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }
    private Vector2 currentDirection = Vector2.zero;
}
