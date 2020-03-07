using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SinBulletMod", menuName = "ScriptableObject/BulletModifier/SinBulletMod", order = 1)]
public class SinMovementMod : BulletModifier
{
    public bool isCos = false;

    public float frequency = 10.0f; // Скорость виляния по синусоиде
    public float R = 1.5f; // Размер синусоиды (радиус, по сути..можно заменить на "R")

    private Vector3 axis;
    private Vector3 pos;

    public override void SpawnModifier(BulletLife bullet)
    {
        base.SpawnModifier(bullet);
        if (!isCos && !bullet.copiedBullet)
        {
            var newBullet = bullet.BulletFullCopy().GetComponent<BulletLife>();
            var sinMod = newBullet.bulletMods.Find(x => x.GetType() == typeof(SinMovementMod)) as SinMovementMod;
            sinMod.isCos = true;
        }

        pos = bullet.transform.position;
        axis = bullet.transform.up;
    }

    public override void MoveModifier(BulletLife bullet)
    {
        base.MoveModifier(bullet);
        pos = bullet.transform.position;
        if (!isCos)
            bullet.transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * R;
        else
            bullet.transform.position = pos + axis * Mathf.Sin(Time.time * frequency + Mathf.PI) * R;
    }
}
