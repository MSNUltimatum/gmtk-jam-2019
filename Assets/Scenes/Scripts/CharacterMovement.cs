using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    private readonly float SpeedBySecond = 250;

    private Vector2 movement;
    private SpriteRenderer CharacterSprite;

    private bool isHorisontal = false;

    private void Start()
    {
        CharacterSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 direction = new Vector2();
        direction += new Vector2(Input.GetAxis("Horizontal"), 0);
        direction += new Vector2(0, Input.GetAxis("Vertical"));
        direction.Normalize();
        
        Movement(direction);
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    private void Movement(Vector2 direction)
    {
        transform.Translate(direction * speed / SpeedBySecond);
    }

    private void FlipXSprite(bool FleepX)
    {
       // if()
    }

    private void FlipYSprite(bool FleepY)
    {
        CharacterSprite.flipY = FleepY;
    }

}
