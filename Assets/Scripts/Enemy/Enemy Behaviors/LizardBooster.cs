using UnityEngine;

public class LizardBooster : Attack
{
    [SerializeField]
    private float boostedSpeed = 5.0f;

    [SerializeField]
    private float boostTime = 2.0f;

    protected override void Awake()
    {
        base.Awake();
        baseSpeed = agent.maxSpeed;
        boostTimeLeft = 0.0f;
    }

    protected override void DoAttack()
    {
        var audio = GetComponent<AudioSource>();
        AudioManager.Play("LizardRun", audio);
            
        boostTimeLeft = boostTime;
        agent.maxSpeed = boostedSpeed;
    }

    public override void CalledUpdate()
    {
        cooldownTimeLeft = Mathf.Max(cooldownTimeLeft - Time.deltaTime, 1);
        boostTimeLeft = Mathf.Max(boostTimeLeft - Time.deltaTime, 0);

        if (boostTimeLeft <= 0)
        {
            agent.maxSpeed = baseSpeed;
        }
    }

    private float baseSpeed;
    private float cooldownTimeLeft;
    private float boostTimeLeft;
}
