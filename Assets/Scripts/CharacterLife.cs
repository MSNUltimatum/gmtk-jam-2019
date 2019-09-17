using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLife : MonoBehaviour
{
    public static bool isDeath = false;

    public void Death()
    {
        CircleCollider2D collider2D = GetComponent<CircleCollider2D>();
        CharacterShooting shooting = GetComponent<CharacterShooting>();
        CharacterMovement movement = GetComponent<CharacterMovement>();
        movement.enabled = false;
        collider2D.isTrigger = true;
        shooting.enabled = false;
        isDeath = true;
    }

    public void Alive()
    {
        CircleCollider2D collider2D = GetComponent<CircleCollider2D>();
        CharacterShooting shooting = GetComponent<CharacterShooting>();
        CharacterMovement movement = GetComponent<CharacterMovement>();
        movement.enabled = true;
        collider2D.isTrigger = false;
        shooting.enabled = true;
        isDeath = false;
    }

}
