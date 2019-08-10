using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletLife : MonoBehaviour
{
    [SerializeField]
    public float BulletSpeed = 12f;
    private GameObject game;
    private RelodScene scenes;

    private void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController");
        scenes = game.GetComponent<RelodScene>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * BulletSpeed * Time.deltaTime);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Destroy(coll.gameObject);
            Time.timeScale = 0;
            scenes.PressR();
        }

        if (coll.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }
}
