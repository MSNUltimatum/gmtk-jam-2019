using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    private GameObject player = null;
    public float destanceToPickup = 1f;
    public float inactiveTime = 0.5f;
    private bool active = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!active)
        {
            inactiveTime -= Time.deltaTime;
            if (inactiveTime <= 0) active = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.gameObject == player)
            PickUp();
    }

    void PickUp() {
        player.GetComponentInChildren<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color; // effect for test, replace it when needed
        Destroy(gameObject);
    }
}
