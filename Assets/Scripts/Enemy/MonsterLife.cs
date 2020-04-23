using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterLife : MonoBehaviour
{
    [SerializeField] public float maxHP = 1;
    [HideInInspector] public float HP = 1;

    [SerializeField] protected GameObject absorbPrefab = null;
    [SerializeField] private GameObject enemyExplosionPrefab = null;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private bool autoChooseName = true;

    [SerializeField] private EvilDictionary evilDictionary = null;

    // Apply listeners on start!!
    public static UnityEvent OnEnemyDead = new UnityEvent();
    public UnityEvent hpChangedEvent = new UnityEvent();

    [HideInInspector] public MonsterManager monsterManager = null;

    protected virtual bool Vulnurable()
    {
        return isBoy();
    }

    private void Awake()
    {
        HP = maxHP;
        FadeIn(fadeInTime);
        sprites = GetComponentsInChildren<SpriteRenderer>();
        monsterName = GetComponentInChildren<TMPro.TextMeshPro>();
        ChooseMyName();
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

        //if (fadeInLeft == 0)
        //{
        //    foreach (var collider in GetComponentsInChildren<Collider2D>())
        //    {
        //        collider.enabled = true;
        //    }
        //}
    }

    protected virtual bool SpecialConditions(GameObject source)
    {
        return true;
    }

    protected virtual void HitEffect() { }

    public void Damage(GameObject source, float damage = 1, bool ignoreInvulurability = false)
    {
        if (HP <= 0) return; // Already dead
        if ((THE_BOY && Vulnurable() || ignoreInvulurability) && SpecialConditions(source))
        {
            var wasHp = HP;
            HP = Mathf.Max(minHpValue, HP - damage);
            if (wasHp != HP) hpChangedEvent?.Invoke();
            else UndamagedAnimation();
            if (HP <= 0)
            {
                if (monsterManager != null)
                    monsterManager.Death(gameObject);
                // Trigger an event for those who listen to it (if any)
                OnEnemyDead?.Invoke();
                PreDestroyEffect();
                OnThisDead?.Invoke();
                Destroy(gameObject);
            }
            else
            {
                HitEffect();
            }
        }
        else
        {
            BulletAbsorb();
        }
    }

    protected virtual void PreDestroyEffect()
    {
        usedNames.Remove(monsterName.text);
        var enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
    }

    public void FadeIn(float _fadeInTime)
    {
        //foreach (var collider in GetComponentsInChildren<Collider2D>())
        //{
        //    collider.enabled = false;
        //}
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
            coll.gameObject.GetComponent<CharacterLife>().Damage();
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

    private void ChooseMyName()
    {
        if (!autoChooseName) return;
        List<string> possibleNames = evilDictionary.EvilNames();
        for (int i = 0; i < 200; i++) // Any ideas how to make this better?
        {
            var possibleName = possibleNames[Random.Range(0, possibleNames.Count)];
            if (!usedNames.Contains(possibleName))
            {
                usedNames.Add(possibleName);
                monsterName.text = possibleName;
                break;
            }
        }
    }

    public static void ClearUsedNames()
    {
        usedNames = new List<string>();
    }

    private GameObject BulletAbsorb()
    {
        if (absorbPrefab)
        {
            var absorb = Instantiate(absorbPrefab, gameObject.transform.position, Quaternion.identity);
            absorb.transform.SetParent(gameObject.transform);
            Destroy(absorb, 0.5f);
            return absorb;
        }
        return null;
    }
    
    /// <param name="percentage01Range">Should be between 0 and 1</param>
    public void SetMinHpPercentage(float percentage01Range)
    {
        if (percentage01Range > 1) Debug.LogError("Percentage parameter should be in range [0, 1]");
        SetMinHpValue(maxHP * percentage01Range);
    }

    public void SetMinHpValue(float minValue)
    {
        if (invulnurabilityShield)
        {
            Destroy(invulnurabilityShield);
        }
        minHpValue = minValue;
    }

    private void UndamagedAnimation()
    {
        if (!invulnurabilityShield && absorbPrefab)
        {
            invulnurabilityShield = Instantiate(absorbPrefab, gameObject.transform.position, Quaternion.identity);
            invulnurabilityShield.transform.SetParent(gameObject.transform);
            invulnurabilityShield.GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
    }

    private float minHpValue = 0;
    
    private float fadeInLeft;
    private SpriteRenderer[] sprites;
    private bool THE_BOY = false;
    private TMPro.TextMeshPro monsterName;
    private static List<string> usedNames = new List<string>();
    private GameObject invulnurabilityShield = null;

    public UnityEvent OnThisDead = new UnityEvent();
}
