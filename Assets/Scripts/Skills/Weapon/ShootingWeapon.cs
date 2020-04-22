using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Simple Shooting Weapon", menuName = "ScriptableObject/Weapon/Simple Shooting Weapon", order = 1)]
public class ShootingWeapon : WeaponSkill
{
    public GameObject bulletPrefab;
    public float bulletDamage = 5;
    public float knockPower = 20f;
    public float bulletSpeed = 18f;
    public float timeToBulletDestruction = 1.2f;
    public float maxRndShootingAngle = 0;
    public float rndShootingAngleAmplifier = 0.15f;
    public float rndShootingAngleRelease = 0.5f;
    [System.NonSerialized]
    public GameObject currentBulletPrefab;
    public static UnityEvent shootingEvents;
    public BulletModifier[] bulletModifiers;

    public override void InitializeSkill()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        currentBulletPrefab = bulletPrefab;
        shootingEvents = new UnityEvent();
        randomShootingAngle = 0;
    }

    public override void Attack(CharacterShooting attackManager, Vector3 mousePos)
    {
        ShootingWeaponAttack(attackManager, mousePos, attackManager.weaponTip);
        AddToRandomAngle();
        shootingEvents?.Invoke();
    }

    public virtual void ShootingWeaponAttack(CharacterShooting attackManager, Vector3 mousePos, Transform shotFrom)
    {
        SpawnBulletTowardsCursor(mousePos, shotFrom, GetRandomAngle(RandomAngleMode.GAUSSIAN));
    }

    public void SpawnBulletTowardsCursor(Vector3 mousePos, Transform shotFrom, float RandomAngle, float additionalAngleOffset = 0)
    {
        var bullet = GameObject.Instantiate(currentBulletPrefab, shotFrom.position, Quaternion.Euler(0, 0, shotFrom.rotation.eulerAngles.z + 90 + GetRandomAngle()));
        BulletInit(bullet);
    }

    public enum RandomAngleMode {
        GAUSSIAN,
    }

    private float GetRandomAngle(RandomAngleMode mode = RandomAngleMode.GAUSSIAN)
    {
        switch (mode)
        {
            case RandomAngleMode.GAUSSIAN:
                return GaussianRandom(0, Mathf.Pow(randomShootingAngle, 0.7f));
            default:
                return 0;
        }
    }

    private void AddToRandomAngle()
    {
        randomShootingAngle = Mathf.Min(maxRndShootingAngle, randomShootingAngle + rndShootingAngleAmplifier * maxRndShootingAngle);
    }

    protected float GaussianRandom(float mean, float deviation)
    {
        float u1 = 1f - Random.Range(0, 1f); //uniform(0,1] random doubles
        float u2 = 1f - Random.Range(0, 1f);
        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) *
                     Mathf.Sin(2f * Mathf.PI * u2); //random normal(0,1)
        return mean + deviation * randStdNormal; //random normal(mean,stdDev^2)
    }

    public override void UpdateEffect()
    {
        base.UpdateEffect();
        randomShootingAngle = Mathf.Max(0, randomShootingAngle - maxRndShootingAngle * rndShootingAngleRelease * Time.deltaTime);
    }

    protected void BulletInit(GameObject bullet)
    {
        BulletLife bulletLife = bullet.GetComponent<BulletLife>();
        if (bulletLife)
        {
            bulletLife.knockThrust = knockPower;
            bulletLife.damage = bulletDamage;
            bulletLife.speed = bulletSpeed;
            bulletLife.timeToDestruction = timeToBulletDestruction;
            foreach (var mod in bulletModifiers)
            {
                bulletLife.AddMod(mod);
            }
            foreach (var mod in SkillManager.temporaryBulletMods)
            {
                bulletLife.AddMod(mod);
            }
        }
        for(int i = 0; i < bullet.transform.childCount; i++)
        {
            BulletInit(bullet.transform.GetChild(i).gameObject);
        }
    }

    public virtual float GunfirePower()
    {
        return bulletDamage / 5 + timeBetweenAttacks / 10 - bulletSpeed / 100 + knockPower / 300;
    }

    public virtual float GunfireDestructivePower()
    {
        return Mathf.Sqrt(bulletDamage * timeBetweenAttacks);
    }

    protected GameObject Player;
    protected float randomShootingAngle = 0;
}
