using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CircularStrikeOnDeathMod", menuName = "ScriptableObject/BulletModifier/CircularStrikeOnDeath", order = 1)]
public class CircularStrikeOnDeathMod : BulletModifier
{
    static List<MonsterLife> taggedEnemies = new List<MonsterLife>(); 

    [SerializeField, Range(2, 32)]
    private int bulletCount = 4;
    [SerializeField, Range(0, 359)]
    private int angleOffset = 45;

    public override void KillModifier(BulletLife bullet, MonsterLife enemy)
    {
        if (taggedEnemies.Contains(enemy)) return;
        taggedEnemies.Add(enemy);

        base.KillModifier(bullet, enemy);
        for (int i = 0; i < bulletCount; i++)
        {
            var newBullet = bullet.BulletFullCopy();
            newBullet.transform.position = enemy.transform.position;
            newBullet.transform.rotation = Quaternion.Euler(0, 0, angleOffset + (360 / bulletCount) * i);
        }
    }
}
