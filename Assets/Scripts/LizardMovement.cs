using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardMovement : EnemyMovement
{
    private float CoolDownBefore;
    [SerializeField]
    private float LizardBoostSpeed = 4.5f;
    protected override void Start()
    {
        CoolDownBefore = Rand();
        base.Start();
    }

    protected override void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        base.Update();
    }

    protected override void MoveToward()
    {
        ExtraSpeed();
        base.MoveToward();
    }

    protected override void Rotation()
    {
        base.Rotation();
    }

    private void ExtraSpeed()
    {
        if (CoolDownBefore < 2f)
        {
            if (!soundLock) {
                var audio = GetComponent<AudioSource>();
                audio.Play();
                soundLock = true;
            }
            EnemySpeed = LizardBoostSpeed;
        }

        if (CoolDownBefore == 0)
        {
            CoolDownBefore = Rand();
            EnemySpeed = 3f;
            soundLock = false;
        }
    }
    private float Rand()
    {
        return Random.Range(4f, 12f);
    }

    private bool soundLock = false;
}
