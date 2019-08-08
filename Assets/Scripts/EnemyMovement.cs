using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    protected float EnemySpeed = 2f;
    protected GameObject Player;
    private SpriteRenderer sprite;

    private AudioSource[] sounds;
    private AudioSource noise1;
    private AudioSource noise2;

    protected virtual void Start()
    {
        sounds = GetComponents<AudioSource>();
        noise1 = sounds[0];
        noise2 = sounds[1];
        noise1.Play();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        MoveToward();
        Rotation();
        noise2.Play();
    }

    protected virtual void MoveToward()
    {      
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, EnemySpeed * Time.deltaTime);
    }

    protected virtual void Rotation()
    {
        float z = Mathf.Atan2((Player.transform.position.y - transform.position.y), (Player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, z);
    }
}
