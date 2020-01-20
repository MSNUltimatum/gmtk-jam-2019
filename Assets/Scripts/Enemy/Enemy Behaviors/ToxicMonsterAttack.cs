using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicMonsterAttack : Shoot
{
    [SerializeField]
    private GameObject SecondAttackBullet = null;
    [SerializeField]
    private float CooldownToSecondAttack = 0.5f;
    [SerializeField, Range(0.25f, 2)]
    private float angleFactor = 0.7f;
    [SerializeField]
    private int Attack2BulletCount = 3;

    [SerializeField]
    private GameObject Orbital = null;
    [SerializeField]
    private int OrbitalCount = 3;

    protected void Start()
    {
        SpawnOrbitals();
    }

    private void SpawnOrbitals()
    {
        for (int i = 0; i < OrbitalCount; i++)
        {
            var orb = Instantiate(Orbital, transform.position, Quaternion.identity);
            var angle = Mathf.Lerp(0, 360, i / (OrbitalCount - 1f));
            orb.GetComponent<OrbitalProtector>().SetHost(gameObject, angle);
        }
    }

    // Update is called once per frame
    protected override void DoAttack()
    {
        ShootBulletStraight(target.transform.position, bullet, 0f);
        StartCoroutine(SecondAttackAfterTimer(CooldownToSecondAttack));
    }

    private IEnumerator SecondAttackAfterTimer(float wait)
    {
        yield return new WaitForSeconds(wait);

        SecondAttack();
    }

    private void SecondAttack()
    {
        var distanceToPlayer = Vector2.Distance(target.transform.position, gameObject.transform.position);
        for (int i = 0; i < Attack2BulletCount; i++)
        {
            var curAngleFactor = Mathf.Lerp(-angleFactor * distanceToPlayer, angleFactor * distanceToPlayer, 
                i / (Attack2BulletCount - 1.0f));
            ShootBulletStraight(target.transform.position + transform.right * curAngleFactor, SecondAttackBullet, 0f);
        }
    }
}
