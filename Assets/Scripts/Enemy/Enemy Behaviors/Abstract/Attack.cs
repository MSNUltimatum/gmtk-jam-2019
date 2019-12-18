using UnityEngine;

public abstract class Attack : EnemyBehavior
{
    [SerializeField]
    protected Vector2 cooldownRange = new Vector2(1f, 1f);

    protected override void Awake()
    {
        base.Awake();
        cooldownLeft = Random.Range(cooldownRange.x, cooldownRange.y);
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        cooldownLeft = Mathf.Max(cooldownLeft - Time.deltaTime, 0);
        if (cooldownLeft <= 0)
        {
            DoAttack();
            cooldownLeft = Random.Range(cooldownRange.x, cooldownRange.y);
        }
    }

    public void ForceAttack()
    {
        DoAttack();
        cooldownLeft = Random.Range(cooldownRange.x, cooldownRange.y);
    }

    protected abstract void DoAttack();

    protected float cooldownLeft;
}