using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float EnemySpeed = 2f;

    private GameObject Player;
    private SpriteRenderer sprite;

    [SerializeField]
    private float CoolDown = 6f;
    float m_CoolDownTL;

    private void Start()
    {
        m_CoolDownTL = CoolDown;
        sprite = GetComponentInChildren<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
        var offset = Player.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    private void Update()
    {
        m_CoolDownTL = Mathf.Max(m_CoolDownTL - Time.deltaTime , 0);
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
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    private void ExtraSpeed()
    {
        if (m_CoolDownTL < CoolDown * 0.4f)
        {
            EnemySpeed = 3.5f;
        }

        if(m_CoolDownTL == 0)
        {
            m_CoolDownTL = CoolDown;
            EnemySpeed = 2f;
        }
    }

    private void ExtraStop()
    {

    }
}
