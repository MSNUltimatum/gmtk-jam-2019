using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemyBullet : EnemyBulletLife
{
    [SerializeField] private float HomingEulerAnglesPerSecond = 45f;
    [SerializeField, Range(0, 1)] private float minimumMagneticPower = 0.2f;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        RotateToPlayer();
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

    private void RotateToPlayer()
    {
        var PlayerPos = Player.transform.position;
        var offset = new Vector2(PlayerPos.x - transform.position.x, PlayerPos.y - transform.position.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        var currentAngle = gameObject.transform.rotation.eulerAngles.z;
        var difference = angle180fix(angle - currentAngle);
        var differenceSign = Mathf.Sign(difference);
        var differenceMaxPerSecond = 
            Mathf.Clamp((180 - difference) / 180, minimumMagneticPower, 1) 
            * differenceSign 
            * HomingEulerAnglesPerSecond 
            * Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, currentAngle + differenceMaxPerSecond);
    }

    private GameObject Player;
}
