using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLife : MonoBehaviour
{
    // Logic
    [System.NonSerialized]
    public float Speed;
    [System.NonSerialized]
    public float timeToDestruction;
    protected float TTDLeft = 0;

    void Start()
    {
        var audio = GetComponent<AudioSource>();
        AudioManager.Play("WeaponShot", audio);
        TTDLeft = timeToDestruction;
    }

    void FixedUpdate()
    {
        if (Pause.Paused) return;
        TTDLeft -= Time.fixedDeltaTime;
        Move();
        if (TTDLeft < 0)
        {
            DestroyBullet();
        }
    }
    
    public float knockThrust = 10;
    public float knockTime = 0.5f;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            EnemyCollider(coll);
        }
        else if (coll.gameObject.tag == "Environment")
        {
            EnvironmentCollider(coll);
        }
    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.right * Speed * Time.fixedDeltaTime);
        TTDLeft -= Time.fixedDeltaTime;
    }

    protected virtual void EnemyCollider(Collider2D coll)
    {
          // KnockBack
          Rigidbody2D Enemy = coll.GetComponent<Rigidbody2D>();
          if (Enemy != null)
          {
              Enemy.drag = 1;
              Vector2 direction = Enemy.transform.position - transform.position;
              direction = direction.normalized * knockThrust;
              Enemy.AddForce(direction, ForceMode2D.Impulse);

              var moveComps = Enemy.GetComponentsInChildren<AIAgent>();
              foreach (var moveComp in moveComps)
              {
                  moveComp.StopMovement(knockTime);
              }
          }

          // Damage
          var monsterComp = coll.gameObject.GetComponent<MonsterLife>();
          if (monsterComp)
          {
              monsterComp.Damage(gameObject);
          }
          else
          {
              Debug.LogError("ОШИБКА: УСТАНОВИТЕ МОНСТРУ " + coll.gameObject.name + " КОМПОНЕНТ MonsterLife");
          }
          DestroyBullet();
    }

    protected virtual void EnvironmentCollider(Collider2D coll)
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
        else if (coll.gameObject.GetComponent<Box>())
        {
            coll.gameObject.GetComponent<Box>().OnBullenHit();
        }
        else
        {
            DestroyBullet();
        }
    }

    public virtual void DestroyBullet()
    {
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<DynamicLightInOut>().FadeOut();
        Destroy(gameObject, 1);
        Destroy(particlesEmitter.gameObject, 2);
        StopEmitter();
    }

    // Non-logic
    [SerializeField]
    private ParticleSystem particlesEmitter = null;
    [SerializeField]
    private SpriteRenderer sprite = null;

    private void StopEmitter()
    {
        particlesEmitter.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        sprite.color = new Color(0, 0, 0, 0);
    }
}
