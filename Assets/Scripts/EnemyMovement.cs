using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float EnemySpeed = 15f;

    private GameObject Player;
    
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        var offset = Player.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    private void Update()
    {
        MoveToward();
        Rotation();
    }

    private void MoveToward()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, EnemySpeed * Time.deltaTime);
    }
    private void Rotation()
    {
        var offset = Player.transform.position - transform.position;
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }
}
