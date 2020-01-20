using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    private GameObject player = null;
    public float destanceToPickup = 1f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < destanceToPickup) {
            PickUp();
        }
    }

    void PickUp() {
        player.GetComponentInChildren<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color; // effect for test, replace it when needed
        Destroy(gameObject);
    }
}
