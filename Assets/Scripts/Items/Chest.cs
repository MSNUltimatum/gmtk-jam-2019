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

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < destanceToOpen)
        {
            //VFX/SFX?
            Open();
        }
    }
}
