using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterLife : MonoBehaviour
{
    [SerializeField]
    public int HP = 1;

    private bool THE_BOY = false;

    [SerializeField]
    private bool allowAutoSelection = true;

    [SerializeField]
    protected GameObject absorbPrefab = null;
    [SerializeField]
    private GameObject enemyExplosionPrefab = null;
    [SerializeField]
    private float fadeInTime = 0.5f;
    
    // Apply listeners on start!!
    public static UnityEvent OnEnemyDead = new UnityEvent();

    protected virtual bool Vulnurable()
    {
        return isBoy();
    }

    private void Start()
    {
        FadeIn(fadeInTime);
        sprites = GetComponentsInChildren<SpriteRenderer>();

        if (absorbPrefab == null)
        {
            absorbPrefab = Resources.Load<GameObject>("AbsorbBubble.prefab");
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
        if ((THE_BOY && Vulnurable() || ignoreInvulurability) && SpecialConditions(source))
        {
            HP -= damage;
            if (HP <= 0)
            {
                // Trigger an event for those who listen to it (if any)
                OnEnemyDead?.Invoke();
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
            coll.gameObject.GetComponent<CharacterLife>().Death();
        }
    }


    public void MakeBoy()
    {
        THE_BOY = true;
        var enemyName = GetComponentInChildren<TMPro.TextMeshPro>();
        if (enemyName == null) return;
        enemyName.sortingLayerID = SortingLayer.NameToID("OnEffect");
        enemyName.sortingOrder = 3;
    }

    public void MakeNoBoy()
    {
        THE_BOY = false;
        var enemyName = GetComponentInChildren<TMPro.TextMeshPro>();
        if (enemyName == null) return;
        enemyName.sortingLayerID = SortingLayer.NameToID("Default");
        enemyName.sortingOrder = 2;
    }

    public bool isBoy()
    {
        return THE_BOY;
    }
    
    private float fadeInLeft;
    private SpriteRenderer[] sprites;
}
