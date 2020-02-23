using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Container
{
    private GameObject player = null;
    public float destanceToOpen = 5f;

    protected override void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        base.Awake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player) {
            //VFX/SFX?
            Open();
            Destroy(gameObject);
        }
    }
}
