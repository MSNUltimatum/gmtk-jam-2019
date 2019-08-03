using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovableEnemy : MonoBehaviour
{
    [SerializeField]
    protected float EnemySpeed = 2f;
    Vector3 direct;
    private float CoolDownBefore;
    private float CoolDown = 1f;
    private SpriteRenderer sprite;
    protected GameObject Player;

    protected void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CoolDownBefore = CoolDown;
        direct = Player.transform.position;
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected  void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        NewDirect();
        MoveToward();
        Rotation();
    }

    protected  void MoveToward()
    {
        transform.position = Vector3.MoveTowards(transform.position, direct, EnemySpeed * Time.deltaTime);
    }

    protected  void Rotation()
    {
        float z = Mathf.Atan2((Player.transform.position.y - transform.position.y), (Player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
       sprite.transform.eulerAngles = new Vector3(0, 0, z);
    }

    private void NewDirect()
    {
        if (CoolDownBefore == 0) { 
            var vect = Player.transform.position + direct / 2;
        vect.Normalize();
        vect.x += Random.Range(-1f, 1f);
        vect.y += Random.Range(-1f, 1f);
        direct = vect + Player.transform.position;
        CoolDownBefore = CoolDown;
        }
        
    }
}
