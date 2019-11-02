using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLife : MonoBehaviour
{
    [SerializeField]
    private int HP = 1;

    bool THE_BOY = false;

    [SerializeField]
    private GameObject absorbPrefab = null;
    [SerializeField]
    private GameObject enemyExplosionPrefab = null;
    [SerializeField]
    private float fadeInTime = 0.5f;

    private void Start()
    {
        FadeIn(fadeInTime);
        sprites = GetComponentsInChildren<SpriteRenderer>();

        if (absorbPrefab == null)
        {
            absorbPrefab = Resources.Load<GameObject>("AbsorbBubble.prefab");
        }

        GameObject target = GameObject.FindGameObjectWithTag("Player");
        Vector2 direction = target.transform.position - transform.position;
        if (direction.magnitude > 0.0f)
        {
            float orientation = Mathf.Atan2(direction.x, direction.y);
            orientation *= Mathf.Rad2Deg;
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.back, orientation);
        }
    }

    private void Update()
    {
        if (Pause.Paused) return;
        if (fadeInLeft != 0) FadeInLogic();
    }

    private void FadeInLogic()
    {
        fadeInLeft = Mathf.Max(fadeInLeft - Time.deltaTime, 0);

        foreach (var sprite in sprites)
        {
            var newColor = sprite.color;
            newColor.a = Mathf.Lerp(1, 0, fadeInLeft / fadeInTime);
            sprite.color = newColor;
        }

        if (fadeInLeft == 0) GetComponent<Collider2D>().enabled = true;
    }

    protected virtual bool SpecialConditions(GameObject source)
    {
        return true;
    }

    protected virtual void HitEffect() { }

    public void Damage(GameObject source, int damage = 1, bool ignoreInvulurability = false)
    {
        if (HP <= 0) return; // Already dead
        if ((THE_BOY || ignoreInvulurability) && SpecialConditions(source))
        {
            HP -= damage;
            if (HP <= 0)
            {
                ArenaEnemySpawner.ChangeTheBoy(gameObject);

                PreDestroyEffect();
                
                Destroy(gameObject);
            }
            else
            {
                HitEffect();
            }
        }
        else
        {
            if (absorbPrefab)
            {
                var absorb = Instantiate(absorbPrefab, gameObject.transform.position, Quaternion.identity);
                absorb.transform.SetParent(gameObject.transform);
                Destroy(absorb, 0.5f);
            }
        }
    }

    protected virtual void PreDestroyEffect()
    {
        var enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(enemyExplosion, 0.5f);
    }

    public void FadeIn(float _fadeInTime)
    {
        GetComponent<Collider2D>().enabled = false;
        fadeInTime = _fadeInTime;
        fadeInLeft = _fadeInTime;
    }

    public float FadeInLeft
    {
        get => fadeInLeft;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (fadeInLeft == 0 && coll.gameObject.tag == "Player")
        {
            CharacterLife life = coll.gameObject.GetComponent<CharacterLife>();
            life.Death();
            RelodScene.PressR();
        }
    }


    public void MakeBoy()
    {
        THE_BOY = true;
    }

    public void MakeNoBoy()
    {
        THE_BOY = false;
    }

    public bool isBoy()
    {
        return THE_BOY;
    }
    
    private float fadeInLeft;
    private SpriteRenderer[] sprites;
}
