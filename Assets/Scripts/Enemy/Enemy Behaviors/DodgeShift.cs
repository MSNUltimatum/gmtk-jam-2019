using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeShift : EnemyBehavior
{
    public float shiftAmp = 1f;

    protected override void Awake()
    {
        base.Awake();
        shotScript = target.GetComponent<CharacterShooting>();
        rigidB = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        
        if (shotScript.shotFrame) {
            float angle = Random.Range(0f, 360f);
            dodgeDirrection = new Vector2(shiftAmp * Mathf.Sin(angle * Mathf.PI / 180f), shiftAmp * Mathf.Cos(angle * Mathf.PI / 180f));
            if (rigidB != null)
            {
                rigidB.AddForce(dodgeDirrection, ForceMode2D.Impulse);
            }
            else {
                Debug.LogError("Error, no Rigidbody2D on monster");
            }
        }
    }

    private CharacterShooting shotScript;
    private Vector2 dodgeDirrection;
    private Rigidbody2D rigidB;
}
