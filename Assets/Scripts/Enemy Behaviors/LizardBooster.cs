using UnityEngine;

public class LizardBooster : EnemyBehavior
{
    [SerializeField]
    private float boostedSpeed = 5.0f;

    [SerializeField]
    private float cooldownTime = 6.0f;

    [SerializeField]
    private float boostTime = 2.0f;

    public override void Awake()
    {
        base.Awake();
        baseSpeed = agent.maxSpeed;
        cooldownTimeLeft = cooldownTime;
        boostTimeLeft = 0.0f;
        boost = false;
    }

    public override void Update()
    {
        cooldownTimeLeft = Mathf.Max(cooldownTimeLeft - Time.deltaTime, 0);
        boostTimeLeft = Mathf.Max(boostTimeLeft - Time.deltaTime, 0);

        if (boostTimeLeft <= 0)
        {
            agent.maxSpeed = baseSpeed;
        }

        if (cooldownTimeLeft <= 0)
        {
            var audio = GetComponent<AudioSource>();
            AudioManager.Play("LizardRun", audio);

            cooldownTimeLeft = cooldownTime;
            boostTimeLeft = boostTime;
            agent.maxSpeed = boostedSpeed;
        }
    }

    private float baseSpeed;
    private float cooldownTimeLeft;
    private float boostTimeLeft;
    private bool boost;
}
