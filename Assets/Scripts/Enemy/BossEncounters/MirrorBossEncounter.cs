using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBossEncounter : BossEncounter
{
    public GameObject explosionProjectile = null;
    [SerializeField] public GameObject bossPrefab = null;
    [HideInInspector] public Transform player;
    [HideInInspector] public Transform bossInstance;

    private class SpawnBossAttack : BossAttack
    {
        public SpawnBossAttack(BossEncounter bossData, float attackLength, bool allowInterruption = true, bool ended = false) : base(bossData, attackLength, allowInterruption, ended)
        {
            this.bossData = bossData as MirrorBossEncounter;
        }

        protected override void AttackStart()
        {
            Instantiate(bossData.bossPrefab, bossData.transform.position, Quaternion.identity);
        }

        private MirrorBossEncounter bossData;
    }

    private class ExplosionAttack : BossAttack
    {
        public ExplosionAttack(BossEncounter bossData, float attackLength, int projectilesCount, bool returnBack = true, bool allowInterruption = true, bool ended = false) 
            : base(bossData, attackLength, allowInterruption, ended)
        {
            this.projectilesCount = projectilesCount; 
            this.bossData = bossData as MirrorBossEncounter;
        }

        protected override void AttackStart()
        {
            projectilePrefab = bossData.explosionProjectile;
            bossInstance = bossData.bossInstance;

            base.AttackStart();

            Vector3 toPlayer = bossData.transform.position - bossData.player.position.normalized;
            Quaternion rotationToPlayer = Quaternion.LookRotation(toPlayer, Vector3.forward);
            Vector3 rotationEulerAngles = new Vector3(0, 0, rotationToPlayer.eulerAngles.z);

            for (int i = 0; i < projectilesCount; i++)
            {
                projectiles.Add(Instantiate(bossData.explosionProjectile, bossInstance.position, Quaternion.Euler(rotationEulerAngles)).transform);
            }
        }

        private int projectilesCount = 0;
        private GameObject projectilePrefab;
        private List<Transform> projectiles;
        private MirrorBossEncounter bossData;
        private Transform bossInstance;
    }

    public class InitialPhase : BossPhase
    {
        public InitialPhase(BossEncounter bossData) : base(bossData)
        {
            phaseName = "Initial phase";
            phaseLength = 19;
            phaseType = PhaseType.TimeBased;
            attackOrder = AttackOrder.Sequence;
            attacks = new List<BossAttack>()
            {
                new SpawnBossAttack(bossData, 1),
                new ExplosionAttack(bossData, 1.5f, 32),
                new ExplosionAttack(bossData, 1.5f, 32),
                new ExplosionAttack(bossData, 1.5f, 32, returnBack: false),
            };
        }
    }

    protected override void Start()
    {
        bossPhases = new List<BossPhase>()
        {
            new InitialPhase(this)
        };

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void StartFight()
    {
        encounterStarted = true;
        base.Start();
    }
}
