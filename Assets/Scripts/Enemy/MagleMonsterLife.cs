using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagleMonsterLife : MonsterLife
{
    [SerializeField] private float additionalSpeedMult = 3f;
    [SerializeField] private float additionalStabilityMult = 2f;
    private bool shieldOperational = true;

    [SerializeField] private Animator spriteAnimation = null;
    [SerializeField] private Animator shadowAnimation = null;

    protected override void Start()
    {
        base.Start();
        shieldReflect = GetComponentInChildren<ReflectBullets>();
    }

    protected override void CustomUpdate()
    {
        if (shieldOperational && isBoy())
        {
            spriteAnimation.Play("Maggle-start-run");
            shieldReflect.enabled = false;
            shieldOperational = false;
            var aiAgent = GetComponent<AIAgent>();
            aiAgent.moveSpeedMult *= additionalSpeedMult;
            aiAgent.knockBackStability *= additionalStabilityMult;
            shieldReflect.gameObject.SetActive(false);
        }
        else if (!shieldOperational && !isBoy())
        {
            spriteAnimation.Play("Maggle-stop-roll");
            shieldReflect.enabled = true;
            shieldOperational = true;
            shieldReflect.gameObject.SetActive(true);
            var aiAgent = GetComponent<AIAgent>();
            aiAgent.moveSpeedMult /= additionalSpeedMult;
            aiAgent.knockBackStability /= additionalStabilityMult;
        }
    }

    private ReflectBullets shieldReflect = null;
}
