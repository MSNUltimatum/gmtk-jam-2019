using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MawEnemyMovement : EnemyMovement
{
    [SerializeField]
    private float DistanceToActivate = 5f;
    [SerializeField]
    private float TimeToActivate = 0.5f;
    private float TTALeft;

    [SerializeField]
    private Color OpenedColor = Color.green;
    [SerializeField]
    private Color BaseColor = Color.yellow;

    [SerializeField]
    new private SpriteRenderer sprite;

    private MawMonsterLife monsterLife;

    protected override void Start()
    {
        base.Start();
        monsterLife = GetComponent<MawMonsterLife>();
        TTALeft = TimeToActivate;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(Player.transform.position, gameObject.transform.position) <= DistanceToActivate)
        {
            TTALeft -= Time.deltaTime;
            
        }
        else
        {
            TTALeft = TimeToActivate;
            monsterLife.Opened = false;
            sprite.color = BaseColor;   // Close animation
        }

        if (TTALeft < 0)
        {
            monsterLife.Opened = true;
            sprite.color = OpenedColor; // Open animation 
        }
    }
}
