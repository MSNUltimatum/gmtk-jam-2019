using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float EnemySpeed = 2f;

    private GameObject Player;
    private SpriteRenderer sprite;

    private float CoolDownBefore;

    private void Start()
    {
        CoolDownBefore = Rand();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");

        var offset = Player.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    private void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime , 0);
        MoveToward();
        Rotation();

    }

    private void MoveToward()
    {
        ExtraSpeed();
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, EnemySpeed * Time.deltaTime);
    }

    private void Rotation()
    {
        var offset = Player.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    private void ExtraSpeed()
    {
        if (CoolDownBefore < 2f)
        {
            EnemySpeed = 3.5f;
        }

        if(CoolDownBefore == 0)
        {
            CoolDownBefore = Rand();
            Debug.Log(CoolDownBefore);
            EnemySpeed = 2f;
        }
    }
    private float Rand()
    {
        return Random.Range(4f, 7f);
    }
}
