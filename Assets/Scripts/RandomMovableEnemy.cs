using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovableEnemy : MonoBehaviour
{
    [SerializeField]
    protected float EnemySpeed = 2f;
    Vector3 direct;
    private float CoolDownBefore;
    [SerializeField]
    private float CoolDown = 1f;
    [SerializeField]
    private Vector2 RangeOfMotion = new Vector2(0, 5);
    private SpriteRenderer sprite;
    protected GameObject Player;

    [SerializeField]
    private float RandomImpact = 5f;

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
       transform.eulerAngles = new Vector3(0, 0, z);
    }

    private void NewDirect()
    {
        if (CoolDownBefore == 0) {

            if (!soundLock)
            {               
                var audio = GetComponent<AudioSource>();
                AudioManager.Play("Cockroach", audio);
                soundLock = true;
            }
            var vect = Player.transform.position + direct / 2;
            vect.Normalize();
            var distanceBasedValue = RandomImpact * Vector2.Distance(Player.transform.position, transform.position); 
            vect.x += Random.Range(-distanceBasedValue, distanceBasedValue);
            vect.y += Random.Range(-distanceBasedValue, distanceBasedValue);
            direct = vect + Player.transform.position;

            CoolDownBefore = Random.Range(CoolDown / 3, CoolDown);

        }
    }
    private bool soundLock = false;
}
