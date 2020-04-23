using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class BulletLife : MonoBehaviour
{
    // Logic
    [System.NonSerialized]
    public float speed;
    [System.NonSerialized]
    public float timeToDestruction;
    [System.NonSerialized]
    public float damage;
    [System.NonSerialized]
    public float TTDLeft = 0.5f;

    public List<BulletModifier> bulletMods = new List<BulletModifier>();

    public bool piercing = false;
    public bool phasing = false;
    public bool copiedBullet = false;

    protected virtual void Start()
    {
        var audio = GetComponent<AudioSource>();
        bulletLight = GetComponent<Light2D>();
        AudioManager.Play("WeaponShot", audio);
        TTDLeft = timeToDestruction;
        ActivateSpawnMods();
        ApplyModsVFX();
    }

    void FixedUpdate()
    {
        if (Pause.Paused) return;
        TTDLeft -= Time.fixedDeltaTime;
        Move();
        UpdateMods();
        if (TTDLeft < 0)
        {
            DestroyBullet();
        }
    }
    
    [System.NonSerialized]
    public float knockThrust = 10;
    [System.NonSerialized]
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
        ActivateMoveModsBefore();
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
        ActivateMoveModsAfter();
    }

    protected virtual void EnemyCollider(Collider2D coll)
    {
        ActivateHitEnemyMods(coll);

        // Damage
        var monsterComp = coll.gameObject.GetComponent<MonsterLife>();
        if (monsterComp)
        {
            DamageMonster(monsterComp);
        }
        else
        {
            Debug.LogError("ОШИБКА: УСТАНОВИТЕ МОНСТРУ " + coll.gameObject.name + " КОМПОНЕНТ MonsterLife");
        }
        if (!piercing) DestroyBullet();
    }

    public void DamageMonster(MonsterLife monster, float damageMultiplier = 1, BulletModifier initiator = null)
    {
        ActivateDamageEnemyMods(monster);
        monster.Damage(gameObject, damage * damageMultiplier * this.damageMultiplier);
        if (monster.HP <= 0)
        {
            ActivateKillMods(monster);
        }
        if (initiator == null) // if the cause of "damage" is not a mod
        {
            // KnockBack
            var enemy = monster.GetComponent<AIAgent>();
            if (enemy != null)
            {
                Vector2 direction = enemy.transform.position - transform.position;
                direction = direction.normalized * knockThrust * Time.fixedDeltaTime;
                enemy.velocity += direction;
            }
        }
    }

    // Bullet mods

    // Instantiates bullet mod and adds to mod list
    public void AddMod(BulletModifier mod)
    {
        bulletMods.Add(Instantiate(mod));
        listNotSorted = true;
    }

    private void UpdateMods()
    {
        for (int i = 0; i < bulletMods.Count; i++) 
        {
            bulletMods[i].ModifierUpdate(this);

            if (bulletMods[i].modifierTime <= 0)
            {
                bulletMods.RemoveAt(i);
                i--;
                listNotSorted = true;
            }
        }
    }

    private List<BulletModifier> SortedMods() {
        if (listNotSorted)
        {
            bulletMods.Sort((x, y) => x.priority.CompareTo(y.priority));
            listNotSorted = false;
        }
        return bulletMods;
    }

    private void ActivateHitEnemyMods(Collider2D coll)
    {
        foreach (var mod in SortedMods()) mod.HitEnemyModifier(this, coll);
    }

    private void ActivateHitEnvironmentMods(Collider2D coll)
    {
        foreach (var mod in SortedMods()) mod.HitEnvironmentModifier(this, coll);
    }

    private void ActivateDamageEnemyMods(MonsterLife enemy, BulletModifier initiator = null)
    {
        foreach (var mod in SortedMods())
        {
            if (mod != initiator) mod.DamageEnemyModifier(this, enemy);
        }
    }

    private void ActivateSpawnMods()
    {
        foreach (var mod in SortedMods()) mod.SpawnModifier(this);
    }

    private void ActivateDestroyMods()
    {
        foreach (var mod in SortedMods()) mod.DestroyModifier(this);
    }

    private void ActivateKillMods(MonsterLife enemy)
    {
        foreach (var mod in SortedMods()) mod.KillModifier(this, enemy);
    }

    private void ActivateMoveModsBefore()
    {
        foreach (var mod in SortedMods())
        {
            if (mod.moveTiming == BulletModifier.MoveTiming.Preparation) mod.MoveModifier(this);
        }
    }

    private void ActivateMoveModsAfter()
    {
        foreach (var mod in SortedMods())
        {
            if (mod.moveTiming == BulletModifier.MoveTiming.Final) mod.MoveModifier(this);
        }
    }

    private void ApplyModsVFX()
    {
        foreach (var mod in SortedMods()) mod.ApplyVFX(this);    
        
    }

    protected virtual void EnvironmentCollider(Collider2D coll)
    {
        ActivateHitEnvironmentMods(coll);

        if (coll.gameObject.GetComponent<Box>())
        {
            coll.gameObject.GetComponent<Box>().OnBullenHit();
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
        else if (!phasing)
        {
            DestroyBullet();
        }
    }

    public GameObject BulletFullCopy()
    {
        var bullet = Instantiate(gameObject, transform.position, transform.rotation);
        var bulletComp = bullet.GetComponent<BulletLife>();
        bulletComp.SetTimeLeft(timeToDestruction);
        bulletComp.speed = speed;
        bulletComp.damage = damage;
        bulletComp.copiedBullet = true;
        
        bulletComp.bulletMods = new List<BulletModifier>();
        foreach (var mod in bulletMods)
        {
            bulletComp.AddMod(mod);
        }

        return bullet;
    }

    public void SetTimeLeft(float timeLeft)
    {
        timeToDestruction = timeLeft;
        TTDLeft = timeLeft;
    }

    public virtual void DestroyBullet()
    {
        ActivateDestroyMods();
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<DynamicLightInOut>().FadeOut();
        Destroy(gameObject, 1);
        Destroy(particlesEmitter.gameObject, 2);
        StopEmitter();
    }

    private void StopEmitter()
    {
        particlesEmitter.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        sprite.color = new Color(0, 0, 0, 0);
    }

    public void BlendSecondColor(Color color)
    {
        Color newColor = color / 2 + sprite.color / 2;
        sprite.color = newColor;
        var emitterMain = particlesEmitter.main;
        emitterMain.startColor = newColor;
        bulletLight.color = newColor;
    }

    public void AddToDamageMultiplier(float addValue)
    {
        damageMultiplier += addValue;
    }

    private bool listNotSorted = true;
    private float damageMultiplier = 1f;

    // Non-logic
    [SerializeField]
    private ParticleSystem particlesEmitter = null;
    private Light2D bulletLight;
    public SpriteRenderer sprite = null;
}
