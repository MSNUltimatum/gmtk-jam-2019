using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModifier : ScriptableObject
{
    [Header("Basic Visual effects"), SerializeField, Tooltip("By default its transparency is 0")]
    private Color bulletColor = Color.white;
    [SerializeField]
    private bool shoodPaint = false;
    [SerializeField]
    private Sprite bulletSprite = null;
    [SerializeField]
    private Material bulletMaterial = null;

    [Header("Logic")]
    public int priority = 0;

    public float modifierTime = float.PositiveInfinity;

    public void UpdateMod()
    {
        modifierTime -= Time.deltaTime;
        if (modifierTime < 0) Destroy(this);
    }

    // Should be called every frame
    public virtual void ModifierUpdate(BulletLife bullet) { }

    // Should be called when the bullet hits enemy (damage or not)
    public virtual void HitEnemyModifier(BulletLife bullet, Collider2D coll) { }

    // Should be called when the bullet damages enemy
    public virtual void DamageEnemyModifier(BulletLife bullet, MonsterLife enemy) { }

    // Should be called when the bullet hits a wall
    public virtual void HitEnvironmentModifier(BulletLife bullet, Collider2D coll) { }

    // Should be called when the bullet is spawned
    public virtual void SpawnModifier(BulletLife bullet) { }

    // Should be called when the bullet is destroyed
    public virtual void DestroyModifier(BulletLife bullet) { }

    // Should be called when the bullet had already been spawned
    public virtual void ApplyModifier(BulletLife bullet) { }

    // Should be called when the bullet kills an enemy
    public virtual void KillModifier(BulletLife bullet, MonsterLife enemy) { }

    public enum MoveTiming { Preparation, Final }
    public MoveTiming moveTiming = MoveTiming.Preparation;

    // Should be called every frame for movememnt
    public virtual void MoveModifier(BulletLife bullet) { }

    public virtual void ApplyVFX(BulletLife bullet) {
        if (shoodPaint) bullet.BlendSecondColor(bulletColor);
        if (bulletSprite != null) bullet.sprite.sprite = bulletSprite;
        if (bulletMaterial != null) bullet.sprite.material = bulletMaterial;
    }

    // WIP SEGMENT
    public enum BulletTypes
    {
        Bullet
    }

    public bool BulletAvailability() { return true; }

    public BulletTypes[] supportedBulletTypes = new BulletTypes[1] { BulletTypes.Bullet };
}
