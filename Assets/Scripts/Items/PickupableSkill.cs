using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PickupableSkill : PickupableItem
{
    public SkillBase skill;
    private Sprite sprite;

    protected override void Update()
    {
        base.Update();
        if ( sprite != skill.pickupSprite)
        {
        GetComponent<SpriteRenderer>().sprite = skill.pickupSprite;
        sprite = skill.pickupSprite;
        }
    }
    protected override void PickUp(Collider2D player)
    {
        var skillInstance = Instantiate(skill);
        player.GetComponent<SkillManager>().AddSkill(skillInstance);
        Destroy(gameObject);
    }
}
