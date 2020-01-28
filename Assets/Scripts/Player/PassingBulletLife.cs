using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingBulletLife : BulletLife
{
    public bool passField = true;
    protected override void EnemyCollider(Collider2D coll)
    {
        var monsterComp = coll.gameObject.GetComponent<MonsterLife>();
        var monsterHp = monsterComp.HP;
        if (monsterComp)
        {
            monsterComp.Damage(gameObject);
            if (monsterComp.HP != monsterHp)
            {
                isPass = false;
            }
        }
        else
        {
            Debug.LogError("ОШИБКА: УСТАНОВИТЕ МОНСТРУ " + coll.gameObject.name + " КОМПОНЕНТ MonsterLife");
            Destroy(coll.gameObject);
        }
        if (!isPass && !passField)
        {
            DestroyBullet();
        }
    }
    private bool isPass = true;
}
