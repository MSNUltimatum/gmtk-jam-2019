using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezing : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<FreezingMonsters>().FreezingShoot(other.gameObject);
        }
    }
}
