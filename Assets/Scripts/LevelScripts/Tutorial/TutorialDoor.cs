using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (opened)
        {
            var newc = sprite.color;
            newc.a = Mathf.Max(0, newc.a - Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.GetComponent<KeyIndicator>())
        {
            opened = true;
        }
    }

    private bool opened = false;
    private SpriteRenderer sprite;
}
