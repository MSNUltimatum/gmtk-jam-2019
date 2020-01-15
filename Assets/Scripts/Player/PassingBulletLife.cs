using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingBulletLife : BulletLife
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            var monsterComp = coll.gameObject.GetComponent<MonsterLife>();
            var monsterHp = monsterComp.GetHp;
            if (monsterComp)
            {
                monsterComp.Damage(gameObject);
                if(monsterComp.GetHp != monsterHp)
                {
                    isPass = false;
                }
            }
            else
            {
                Debug.LogError("ОШИБКА: УСТАНОВИТЕ МОНСТРУ " + coll.gameObject.name + " КОМПОНЕНТ MonsterLife");
                Destroy(coll.gameObject);
            }
            if (!isPass)
            {
                DestroyBullet();
            }
        }
        else if (coll.gameObject.tag == "Environment")
        {
            if (coll.gameObject.GetComponent<DestructibleWall>() != null)
            {
                DestroyBullet();
            }
            if (coll.gameObject.GetComponent<MirrorWall>() != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right,
                    float.PositiveInfinity, LayerMask.GetMask("Default"));
                if (hit)
                {
                    Vector2 reflectDir = Vector2.Reflect(transform.right, hit.normal);
                    float rot = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, rot);
                }
            }
            else
            {
                DestroyBullet();
            }
        }
    }
    private bool isPass = true;
}
