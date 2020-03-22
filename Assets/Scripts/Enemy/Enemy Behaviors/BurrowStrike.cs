using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BurrowStrike : Attack
{
    [SerializeField]
    private GameObject burrowEffect = null;
    [SerializeField]
    private List<SpriteRenderer> spritesToFade = null;
    [SerializeField]
    private float burrowTime = 1.5f;
    [SerializeField]
    private float burrowedSpeed = 7f;
    [SerializeField]
    private float attackRadius = 2f;
    [SerializeField]
    private float unburrowTime = 0.5f;
    [SerializeField]
    private float diggingTime = 2f;
    [SerializeField]
    private GameObject clawAttack = null;

    // TODO: No hardcoded colors, please!
    

    protected override void DoAttack()
    {
        cooldownLeft += burrowTime + unburrowTime + diggingTime;

        aiAgent.maxSpeed = 0;
        aiAgent.maxRotation = 0;
        rockDigEffect = Instantiate(burrowEffect, transform);
        rockDigEffect.transform.Translate(-transform.up * 0.8f, Space.World);

        currentState = BurrowState.Burrowing;
        timeToNextState = burrowTime;
    }

    protected void Start()
    {
        startingColor = new List<Vector4>();
        foreach (var sprite in spritesToFade)
        {
            startingColor.Add(sprite.color);
        }
        aiAgent = GetComponent<AIAgent>();
        maxSpeedSaved = aiAgent.maxSpeed;
        maxRotationSaved = aiAgent.maxRotation;
    }

    private enum BurrowState {
        None,
        Burrowing,
        Digging,
        Unburrowing
    }

    // Burrow
    public override void CalledUpdate()
    {
        base.CalledUpdate();
        timeToNextState -= Time.deltaTime;
        switch (currentState)
        {
            case BurrowState.None:
                break;
            case BurrowState.Burrowing:
                for (int i = 0; i < spritesToFade.Count; i++)
                {
                    var currentColor = Color.Lerp(startingColor[i], burrowColor, 1 - (timeToNextState / burrowTime));

                    spritesToFade[i].color = currentColor;
                }

                if (timeToNextState <= 0)
                {
                    if (CheckWallAhead())
                    {
                        Unburrow();
                        return;
                    }
                    aiAgent.maxSpeed = burrowedSpeed;
                    GetComponent<Collider2D>().enabled = false;
                    foreach (var particle in rockDigEffect.GetComponentsInChildren<ParticleSystem>())
                    {
                        var particleMain = particle.main;
                        particleMain.startSize = new ParticleSystem.MinMaxCurve(particleMain.startSize.constant * 0.7f);
                    }

                    currentState = BurrowState.Digging;
                    timeToNextState = diggingTime;
                }
                break;
            case BurrowState.Digging:
                timeToScan -= Time.deltaTime;
                if (timeToScan <= 0)
                {
                    timeToScan = timeToEachScan;
                    if (CheckWallAhead() || CheckEnemyNearby()) Unburrow();
                }

                if (timeToNextState <= 0)
                {
                    Unburrow();
                }
                break;
            case BurrowState.Unburrowing:
                for (int i = 0; i < spritesToFade.Count; i++)
                {
                    spritesToFade[i].color = Color.Lerp(burrowColor, startingColor[i], 1 - (timeToNextState / unburrowTime));
                }

                if (timeToNextState <= 0)
                {
                    CompleteAttack();
                }
                break;
            default:
                break;
        }
    }

    protected void Unburrow()
    {
        currentState = BurrowState.Unburrowing;
        timeToNextState = unburrowTime;

        foreach (var particle in rockDigEffect.GetComponentsInChildren<ParticleSystem>())
        {
            var particleEmission = particle.emission;
            particleEmission.rateOverTime = new ParticleSystem.MinMaxCurve(particleEmission.rateOverTime.constant * 2.5f);
            var particleShape = particle.shape;
            particleShape.angle = particleShape.angle * 4f;
        }
    }

    protected void CompleteAttack()
    {
        currentState = BurrowState.None;
        var clawInstance = Instantiate(clawAttack, transform.position + transform.up * 2f, Quaternion.identity);
        var offset = new Vector2(clawInstance.transform.position.x - transform.position.x, clawInstance.transform.position.y - transform.position.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        clawInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        rockDigEffect.GetComponent<DetachDestroyParticleEmitter>().TimeToDestroy = -0;
        for (int i = 0; i < spritesToFade.Count; i++)
        {
            spritesToFade[i].color = startingColor[i];
        }
        GetComponent<Collider2D>().enabled = true;
        aiAgent.maxSpeed = maxSpeedSaved;
        aiAgent.maxRotation = maxRotationSaved;
    }

    private bool CheckEnemyNearby()
    {
        //objectsNearby = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        //foreach (var obj in objectsNearby)
        //{
        //    if (obj.transform.CompareTag("Player"))
        //    {
        //        return true;
        //    }
        //}
        //return false;
        return Vector3.Distance(target.transform.position, transform.position) <= attackRadius;
    }

    private bool CheckWallAhead()
    {
        //Debug.DrawRay(transform.position, transform.up * attackRadius);
        var hits = (from t in Physics2D.RaycastAll(transform.position, transform.up, attackRadius + 1)
                    where t.transform.gameObject.tag == "Environment"
                    select t).ToArray();
        return (hits.Length != 0);
    }

    private float timeToNextState = Mathf.Infinity;
    private BurrowState currentState;
    
    private GameObject rockDigEffect = null;
    private Collider2D[] objectsNearby;
    private float timeToScan = 0f;
    private float timeToEachScan = 0.25f;

    private List<Vector4> startingColor;
    private float maxSpeedSaved = 0;
    private float maxRotationSaved = 0;
    private Color burrowColor = new Color32(209, 188, 138, 0);
    private AIAgent aiAgent;
}
