using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterMovement : EnemyMovement
{
    [SerializeField]
    private GameObject Bullet;
    private float CoolDownBefore;
    public AudioSource[] sounds;
    public AudioSource noise1;
    public AudioSource noise2;
    private float CoolDown = 1f;
    protected override void Start()
    {
        sounds = GetComponents<AudioSource>();

        noise1 = sounds[0];
        noise2 = sounds[1];

        noise1.Play();

        CoolDownBefore = CoolDown;
        base.Start();
    }

    protected override void Update()
    {        
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        if (CoolDownBefore == 0)
        {            
            CmdShoot(Player.transform.position);
            CoolDownBefore = CoolDown;
        }
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

    private void CmdShoot(Vector3 PlayerPos)
    {
        var bullet = Instantiate(Bullet, transform.position, new Quaternion());

        noise2.Play();

        var offset = new Vector2(PlayerPos.x - transform.position.x, PlayerPos.y - transform.position.y);

        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        angle += Random.Range(-15, 15);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.transform.Translate(Vector2.right * 0.5f);
    }
}
