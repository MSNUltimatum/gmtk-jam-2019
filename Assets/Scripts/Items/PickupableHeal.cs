using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableHeal : PickupableItem
{
    public int healAmount = 1;

    protected override void PickUp(Collider2D player)
    {
        player.GetComponent<CharacterLife>().Heal(healAmount);
        Destroy(gameObject);
    }
}
