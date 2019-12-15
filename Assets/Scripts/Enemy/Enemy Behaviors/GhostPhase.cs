using UnityEngine;

public class GhostPhase : Attack
{
    [SerializeField]
    private float GhostBoostSpeed = 7f;
    [SerializeField]
    private bool PacifistInBoost = true;
    [SerializeField]
    private float BoostTime = 2.5f;

    protected override void Awake()
    {
        base.Awake();
        standardSpeed = agent.maxSpeed;
        BoxCollider = GetComponent<BoxCollider2D>();
        cooldownLeft = cooldownLeft / 2;
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void DoAttack()
    {
        var audio = GetComponent<AudioSource>();
        AudioManager.Play("Ghost", audio);

        BoxCollider.isTrigger = PacifistInBoost;
        agent.maxSpeed = GhostBoostSpeed;
        var s = sprite.color;
        s.a = 0.5f;
        sprite.color = s;

        boostTimeLeft = BoostTime;
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        boostTimeLeft -= Time.deltaTime;
        if (boostTimeLeft <= 0)
        {
            BoxCollider.isTrigger = false;
            agent.maxSpeed = standardSpeed;
            var s = sprite.color;
            s.a = 1f;
            sprite.color = s;
        }
    }

    private float boostTimeLeft;
    private BoxCollider2D BoxCollider;
    private SpriteRenderer sprite;
    private float standardSpeed;
}
