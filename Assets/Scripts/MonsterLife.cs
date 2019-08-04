using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLife : MonoBehaviour
{
    [SerializeField]
    private int HP = 1;

    private GameObject game;
    private RoomLighting Room;
    private RelodScene scenes;
    bool THE_BOY = false;

    private void Update()
    {
        fadeInLeft -= Time.deltaTime;
        if (fadeInLeft <= 0) return;

        var newColor = sprite.color;
        newColor.a = Mathf.Lerp(1, 0, fadeInLeft / fadeInTime);
        sprite.color = newColor;
    }
    
    private void Start()
    {
        fadeInLeft = fadeInTime;
        sprite = GetComponentInChildren<SpriteRenderer>();
        game = GameObject.FindGameObjectWithTag("GameController");
        Room = game.GetComponent<RoomLighting>();
        scenes = game.GetComponent<RelodScene>();
    }

    public void Damage()
    {
        if (THE_BOY)
        {
            HP--;
            GameObject.Find("Game Manager").GetComponent<ArenaEnemySpawner>().ChangeTheBoy(gameObject);
            // TODO: add EXPLOSION, MOTHERF!$&*ER
            if (HP == 0)
            {
                scenes.CurrentCount(1);
                Room.Lighten(1);
                Destroy(gameObject);
            }
        }
        else
        {
            // TODO: make visual and sound effects of absorb
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Destroy(coll.gameObject);
            Time.timeScale = 0;
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
