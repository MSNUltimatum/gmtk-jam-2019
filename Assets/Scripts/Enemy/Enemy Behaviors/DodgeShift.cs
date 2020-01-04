using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeShift : EnemyBehavior
{
    private CharacterShooting shotScript;
    //private bool dodgeMode = false;
    Vector2 dodgeDirrection;
    public float shiftAmp = 1f;
    private Rigidbody2D rigidB;

    protected override void Awake()
    {
        base.Awake();
        shotScript = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterShooting>();
        rigidB = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        
        if (shotScript.shotFrame) {
            float angle = Random.Range(0f, 360f);
            dodgeDirrection = new Vector2( shiftAmp * Mathf.Sin(angle * Mathf.PI / 180f), shiftAmp * Mathf.Cos(angle * Mathf.PI / 180f));
            if (rigidB != null)
            {
                rigidB.AddForce(dodgeDirrection, ForceMode2D.Impulse);
            }
            else {
                Debug.Log("Error, no Rigidbody2D on monster");
            }
        }
    }
}
