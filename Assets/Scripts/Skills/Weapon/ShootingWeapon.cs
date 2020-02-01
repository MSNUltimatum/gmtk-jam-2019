using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingWeapon : WeaponSkill
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 18f;
    public float timeToBulletDestruction = 1.2f;
    [System.NonSerialized]
    public GameObject currentBulletPrefab;

    public override void InitializeSkill()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        currentBulletPrefab = bulletPrefab;
    }

    public override void Attack(CharacterShooting attackManager, Vector3 mousePos, Vector3 screenPoint)
    {
        var bullet = GameObject.Instantiate(currentBulletPrefab, Player.transform.position + Player.transform.right * 0.15f, new Quaternion());
        BulletInit(bullet);
        var offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        angle += Random.Range(-randomShootingAngle, randomShootingAngle);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.transform.Translate(Vector2.right * 0.5f);
    }

    protected void BulletInit(GameObject bullet)
    {
        BulletLife bulletLife = bullet.GetComponent<BulletLife>();
        bulletLife.Speed = bulletSpeed;
        bulletLife.timeToDestruction = timeToBulletDestruction;
    }

    protected GameObject Player;
    protected float randomShootingAngle = 0;
}
