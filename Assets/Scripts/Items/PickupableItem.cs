using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    private GameObject player = null;
    public float destanceToPickup = 1f;
    public float inactiveTime = 0.5f;
    private bool active = false;
    public enum ItemType { skill, temporary };
    public ItemType type;
    public SkillBase skill;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (type == ItemType.skill) {
            GetComponent<SpriteRenderer>().sprite = skill.pickupSprite;
        }
    }

    void Update()
    {
        if (!active)
        {
            inactiveTime -= Time.deltaTime;
            if (inactiveTime <= 0) active = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.gameObject == player)
            PickUp();
    }

    void PickUp() {
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
