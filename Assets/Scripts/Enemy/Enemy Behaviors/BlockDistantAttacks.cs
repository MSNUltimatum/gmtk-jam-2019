using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDistantAttacks : EnemyBehavior
{
    [SerializeField]
    private float DistanceToActivate = 5f;
    [SerializeField]
    private float TimeToActivate = 0.5f;
    [SerializeField]
    private SpriteRenderer mainSprite = null;
    [SerializeField]
    private Color OpenedColor = Color.green;
    [SerializeField]
    private Color BaseColor = Color.yellow;

    protected override void Awake()
    {
        base.Awake();
        monsterLife = GetComponent<MawMonsterLife>();
    }

    public override void CalledUpdate()
    {
        if (Vector3.Distance(target.transform.position, gameObject.transform.position) <= DistanceToActivate)
        {
            TTALeft -= Time.deltaTime;

        }
        else
        {
            TTALeft = TimeToActivate;
            monsterLife.Opened = false;
            mainSprite.color = BaseColor;   // Close animation
        }

        if (TTALeft < 0)
        {
            monsterLife.Opened = true;
            mainSprite.color = OpenedColor; // Open animation 
        }
    }

    private MawMonsterLife monsterLife;
    private float TTALeft;
}
