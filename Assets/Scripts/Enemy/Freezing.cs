using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezing : MonoBehaviour
{
    [SerializeField]
    private float freezingDuration = 3f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var fr = other.gameObject.GetComponent<FreezingMonsters>();
            if(!fr)
            {
                other.gameObject.AddComponent<FreezingMonsters>();
                other.gameObject.GetComponent<FreezingMonsters>().MyStart(freezingDuration);
            }
            else
            {
                fr.Reboot();
            }
        }
    }
}
