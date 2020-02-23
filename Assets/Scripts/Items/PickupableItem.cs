using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableItem : MonoBehaviour
{
    public float destanceToPickup = 1f;
    public float inactiveTime = 0.5f;
    private bool active = false;

    protected virtual void Update()
    {
        if (Application.IsPlaying(gameObject)) {
            if (!active) {
                inactiveTime -= Time.deltaTime;
                if (inactiveTime <= 0) active = true;
            }
        } 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Player"))
           PickUp(collision);
    }

    protected abstract void PickUp(Collider2D player);
}
