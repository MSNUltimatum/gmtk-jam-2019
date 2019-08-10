using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemy : EnemyMovement
{
    [SerializeField]
    private Vector2 TpCooldownRange = new Vector2(5, 7);
    private float CoolDownBefore;
    [SerializeField]
    private float Scatter = 8f;
    protected override void Start()
    {
        CoolDownBefore = Rand();
        base.Start();
    }

    protected override void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        Teleport();
        base.Update();
    }

    protected override void MoveToward()
    {
        base.MoveToward();
    }

    protected override void Rotation()
    {
        base.Rotation();
    }

    private void Teleport()
    {
        if (CoolDownBefore == 0)
        {
            var audio = GetComponent<AudioSource>();

            if (audio)
            {
                audio.Play();
            }

            CoolDownBefore = Rand();
            float Xpos = Random.Range(-100, 100);
            float YPos = Random.Range(-100, 100);
            var vect = new Vector2(Player.transform.position.x - Xpos, Player.transform.position.y - YPos);
            vect.Normalize();
            vect *= Scatter;
            transform.position = Player.transform.position + new Vector3(vect.x, vect.y);
        }
    }

    private float Rand()
    {
        return Random.Range(TpCooldownRange.x, TpCooldownRange.y);
    }
}
