using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PickupableItem : MonoBehaviour
{
    public float destanceToPickup = 1f;
    public float inactiveTime = 0.5f;
    private bool active = false;
    public enum ItemType { skill, temporary };
    public ItemType type;
    public SkillBase skill;
    private Sprite sprite;

    void Update()
    {
        if (Application.IsPlaying(gameObject)) {
            if (!active) {
                inactiveTime -= Time.deltaTime;
                if (inactiveTime <= 0) active = true;
            }
        } 
        if (type == ItemType.skill && sprite != skill.pickupSprite)
        {
            GetComponent<SpriteRenderer>().sprite = skill.pickupSprite;
            sprite = skill.pickupSprite;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Player"))
            PickUp(collision);
    }

    void PickUp(Collider2D player) {
        if (type == ItemType.skill) {
            var skillInstance = Instantiate(skill);
            player.GetComponent<SkillManager>().AddSkill(skillInstance);
        }
        else if (type == ItemType.temporary) {
            //do nothing for now, under construction
        }
        Destroy(gameObject);
    }
}
