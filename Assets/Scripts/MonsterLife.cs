using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLife : MonoBehaviour
{
    [SerializeField]
    private int HP = 1;

    bool THE_BOY = false;

    private void Start()
    {
        fadeInLeft = fadeInTime;
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        fadeInLeft -= Time.deltaTime;
        if (fadeInLeft <= 0) return;

        var newColor = sprite.color;
        newColor.a = Mathf.Lerp(1, 0, fadeInLeft / fadeInTime);
        sprite.color = newColor;
    }
    
    public void Damage()
    {
        if (THE_BOY)
        {
            HP--;
            GameObject.Find("Game Manager").GetComponent<ArenaEnemySpawner>().ChangeTheBoy(gameObject);
            // TODO: add EXPLOSION, MOTHERF!$&*ER
            if(HP == 0)
            Destroy(gameObject);
        }
        else
        {
            // TODO: make visual and sound effects of absorb
        }
    }

    public void MakeBoy()
    {
        THE_BOY = true;
    }

    private float fadeInTime = 0.5f;
    private float fadeInLeft;
    private SpriteRenderer sprite;
}
