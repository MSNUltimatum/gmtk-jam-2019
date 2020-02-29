using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftAfterShoot : EnemyBehavior 
{
    public float shiftAmp = 1f; // multiplier to shift

    protected override void Awake()
    {
        base.Awake();
        moveAgent = gameObject.GetComponent<AIAgent>();
    }

    public void DoShift() {
        float angle = Random.Range(0f, 360f);
        dodgeDirection = new Vector2(shiftAmp * Mathf.Sin(angle * Mathf.PI / 180f), shiftAmp * Mathf.Cos(angle * Mathf.PI / 180f));
        if (moveAgent != null)
        {
            moveAgent.velocity += dodgeDirection;
        }
        else
        {
            Debug.LogError("Error, no Rigidbody2D on monster");
        }
    }

    private Vector2 dodgeDirection;
    private AIAgent moveAgent;
}
