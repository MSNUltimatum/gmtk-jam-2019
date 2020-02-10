using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterLife : MonoBehaviour
{
    [SerializeField]
    public int HP = 1;

    [SerializeField]
    protected GameObject absorbPrefab = null;
    [SerializeField]
    private GameObject enemyExplosionPrefab = null;
    [SerializeField]
    private float fadeInTime = 0.5f;
    
    [SerializeField]
    private EvilDictionary evilDictionary = null;

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
        monsterName = GetComponentInChildren<TMPro.TextMeshPro>();

        ChooseMyName();

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
    
    private float fadeInLeft;
    private SpriteRenderer[] sprites;
    private bool THE_BOY = false;
    private TMPro.TextMeshPro monsterName;
    private static List<string> usedNames = new List<string>();
}
