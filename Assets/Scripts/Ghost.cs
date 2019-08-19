using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyMovement
{
    private float CoolDownBefore;
    [SerializeField]
    private float CoolDown = 18f;
    [SerializeField]
    private float GhostBoostSpeed = 7f;
    [SerializeField]
    private bool ImmortalityInBoost = true;
    private BoxCollider2D BoxCollider;
    private SpriteRenderer sprite;

    private bool soundLock = false;
    protected override void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        CoolDownBefore = CoolDown;
        sprite = GetComponentInChildren<SpriteRenderer>();
        base.Start();
    }

    protected override void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        GhostMode();
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

    private void GhostMode()
    {
        if (CoolDownBefore < 3f)
        {        
            if (!soundLock)
            {
                var audio = GetComponent<AudioSource>();
                AudioManager.Play("Ghost", audio);
                soundLock = true;
            }
            BoxCollider.isTrigger = ImmortalityInBoost;
            EnemySpeed = GhostBoostSpeed;        
            var s = sprite.color;
            s.a = 0.5f;
            sprite.color = s;
        }

        if (CoolDownBefore == 0)
        {
            CoolDownBefore = CoolDown;
            BoxCollider.isTrigger = false;
            EnemySpeed = 4f;
            var s = sprite.color;
            s.a = 1f;
            sprite.color = s;
            soundLock = false;
        }
    }
    
}
