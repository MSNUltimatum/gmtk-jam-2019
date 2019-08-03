using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletLife : MonoBehaviour
{
    public float Speed = 0.5f;

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * Speed);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        /*if (coll.gameObject.tag == "Player")
        {
            var monsterComp = coll.gameObject.GetComponent<MonsterLife>();
            if (monsterComp)
            {
                monsterComp.Damage();
            }
            else
            {
                Debug.LogError("ОШИБКА: УСТАНОВИТЕ МОНСТРУ " + coll.gameObject.name + " КОМПОНЕНТ MonsterLife");
                Destroy(coll.gameObject);
            }
            Destroy(gameObject);
        }

        if (coll.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }*/
    }
}
