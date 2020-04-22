using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfSpawningContainer : Chest
{
    protected override void Awake()
    {
        if (Labirint.instance.blueprints[Labirint.instance.currentRoomID].containerWasOpened)
        {
            Destroy(gameObject);
        }
        base.Awake();
    }
}
