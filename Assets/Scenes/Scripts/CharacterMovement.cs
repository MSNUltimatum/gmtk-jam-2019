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
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rot = Quaternion.LookRotation(transform.position - mousepos, Vector3.forward);
        transform.rotation = rot;
    }
    private void Movement(Vector2 direction)
    {
        transform.Translate(direction * speed / SpeedBySecond,Space.World);
    }

}
