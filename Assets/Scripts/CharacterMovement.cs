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
    private Animator anim;

    private bool isHorisontal = false;

    private void Start()
    {
        CharacterSprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Movement();
        Rotation();
    }
    private void Rotation()
    {
        var mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rot = Quaternion.LookRotation(transform.position - mousepos, Vector3.forward);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }
    private void Movement()
    {
        Vector2 direction = new Vector2();
        direction += new Vector2(Input.GetAxis("Horizontal"), 0);
        direction += new Vector2(0, Input.GetAxis("Vertical"));
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        if (anim != null)
        {
            if (direction.magnitude == 0)
            {
                anim.Play("HeroIdle");
            }
            else
            {
                anim.Play("HeroWalking");
            }
        }
        transform.Translate(direction * speed / SpeedBySecond,Space.World);
    }

}
