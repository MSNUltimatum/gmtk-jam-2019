using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetMonsterMovement : EnemyMovement
{
    private Vector2 direction;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        direction = (Player.transform.position - gameObject.transform.position).normalized;
    }

    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(direction * EnemySpeed * Time.deltaTime);
       
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Environment")
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction,
                    float.PositiveInfinity, LayerMask.GetMask("Default"));
            if (hit)
            {
                direction = Vector2.Reflect(direction, hit.normal);
            }
        }
    }
}
