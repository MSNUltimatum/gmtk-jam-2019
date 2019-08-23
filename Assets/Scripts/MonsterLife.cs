using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLife : MonoBehaviour
{
    [SerializeField]
    protected int HP = 1;

    protected GameObject game;
    protected RoomLighting Room;
    protected RelodScene scenes;
    bool THE_BOY = false;
    
    [SerializeField]
    protected GameObject absorbPrefab = null;
    [SerializeField]
    protected GameObject enemyExplosionPrefab = null;

    protected void Update()
    {
        fadeInLeft -= Time.deltaTime;
        if (fadeInLeft <= 0) return;

        var newColor = sprite.color;
        newColor.a = Mathf.Lerp(1, 0, fadeInLeft / fadeInTime);
        sprite.color = newColor;
    }
    
    protected void Start()
    {
        fadeInLeft = fadeInTime;
        sprite = GetComponentInChildren<SpriteRenderer>();
        game = GameObject.FindGameObjectWithTag("GameController");
        Room = game.GetComponent<RoomLighting>();
        scenes = game.GetComponent<RelodScene>();

        if (absorbPrefab == null)
        {
            absorbPrefab = Resources.Load<GameObject>("AbsorbBubble.prefab");
        }
    }

    protected virtual bool Vulnurable()
    {
        return true;
    }

    public void Damage()
    {
        if (THE_BOY & Vulnurable())
        {
            HP--;
            if (HP == 0)
            {
                GameObject.Find("Game Manager").GetComponent<ArenaEnemySpawner>().ChangeTheBoy(gameObject);
                if (scenes)
                {
                    scenes.CurrentCount(1);
                }
                Room.Lighten(1);
                var enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.5f);
                Destroy(gameObject);
            }
        }
        else
        {
            // TODO: make visual and sound effects of absorb
            if (absorbPrefab)
            {
                var absorb = Instantiate(absorbPrefab, gameObject.transform.position, Quaternion.identity);
                absorb.transform.SetParent(gameObject.transform);
                Destroy(absorb, 0.5f);
            }
        }
    }

    protected void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Destroy(coll.gameObject);
            Time.timeScale = 0;
            scenes.PressR();
        }
    }


    public void MakeBoy()
    {
        THE_BOY = true;
    }

    protected float fadeInTime = 0.5f;
    protected float fadeInLeft;
    protected SpriteRenderer sprite;
}
