using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingWeapon : WeaponSkill
{
    protected float randomShootingAngle = 0;
    public GameObject bulletPrefab;

    protected override List<Object> SaveReferences()
    {
        referencesToSave.Add(bulletPrefab);
        return referencesToSave;
    }

    protected override void LoadReferences(List<Object> references)
    {
        bulletPrefab = references[1] as GameObject;
    }

    public override void InitializeSkill()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Attack(CharacterShooting attackManager, Vector3 mousePos, Vector3 screenPoint)
    {
        var bullet = GameObject.Instantiate(bulletPrefab, Player.transform.position, new Quaternion());

        var offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        angle += Random.Range(-randomShootingAngle, randomShootingAngle);
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.transform.Translate(Vector2.right * 0.5f);
    }

    protected GameObject Player;
}
