using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickupableHealRandomDrop : PickupableHeal
{
   private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var hpMan = player.GetComponent<CharacterLife>();
        var chance = Mathf.Lerp(0.33f, 0.1f, (float)((hpMan.GetHp() - 1) / (hpMan.GetMaxHp() - 1))) * hpMan.GetHpDropChanceAmplifier();
        System.Random rand = new System.Random();
        if (rand.NextDouble() > chance)
        {
            Destroy(gameObject);
        }
    }
}
