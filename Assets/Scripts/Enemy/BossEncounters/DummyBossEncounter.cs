using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyBossEncounter : BossEncounter
{
    [SerializeField] protected GameObject butterfly = null;
    [SerializeField] protected GameObject butterflyPurple = null;
    [SerializeField] protected GameObject butterflyWhite = null;
    [SerializeField] protected GameObject butterflyGreen = null;
    [SerializeField] protected GameObject butterflyRed = null;
    [SerializeField] protected GameObject dragonFly = null;
    [SerializeField] protected GameObject butterflyPack = null;

    public class ButterflyAttack : BossAttack
    {
        public ButterflyAttack(BossEncounter bossData, float waitAfterAttack, GameObject butterfly, bool allowInterruption = true, bool ended = false) 
            : base(bossData, waitAfterAttack, allowInterruption, ended) {
            dummyBossData = bossData as DummyBossEncounter;
            butterflyToSpawn = butterfly;
        }

        protected override void AttackStart()
        {
            base.AttackStart();
            print($"I am creating {butterflyToSpawn.name}! Attack cooldown is: {attackLength}");
            var fly = Instantiate(butterflyToSpawn, dummyBossData.transform.position, Quaternion.identity);
            Destroy(fly, 5f);
        }

        GameObject butterflyToSpawn;
        DummyBossEncounter dummyBossData;
    }

    public class DummyBossPhaseWelcome : BossPhase
    {
        public DummyBossPhaseWelcome(BossEncounter bossData) : base (bossData)
        {
            phaseName = "DummyWelcome";
            phaseLength = 10;
            phaseType = PhaseType.TimeBased;
            attackOrder = AttackOrder.RandomRepeatable;
            dummyBossData = bossData as DummyBossEncounter;
            attacks = new List<BossAttack>() {
                new ButterflyAttack(bossData, 1.79f, dummyBossData.butterflyWhite),
                new ButterflyAttack(bossData, 0.5f, dummyBossData.butterfly),
                new ButterflyAttack(bossData, 1f, dummyBossData.butterflyGreen),
            };
        }

        public override void StartPhase()
        {
            base.StartPhase();
            dummyBossData.bossHP.SetMinHpPercentage(0.75f);
        }

        DummyBossEncounter dummyBossData;
    }

    public class DummyBossEnrage : BossPhase
    {
        public DummyBossEnrage(BossEncounter bossData, float hpUntil, float attackSpeedMultiplier) : base(bossData)
        {
            phaseName = "DummyEnrage";
            phaseLength = 5;
            endHpPercentage = hpUntil;
            phaseType = PhaseType.HpBased;
            attackOrder = AttackOrder.RandomRepeatable;
            dummyBossData = bossData as DummyBossEncounter;
            attacks = new List<BossAttack>() {
                new ButterflyAttack(
                    bossData, 0.5f / attackSpeedMultiplier, 
                    attackSpeedMultiplier > 1 ? dummyBossData.butterflyGreen : dummyBossData.butterflyRed),
            };
        }

        public override void DebugStartPhase()
        {
            AudioManager.PlayMusic(dummyBossData.GetComponent<AudioSource>(), 60);
            var bossHP = dummyBossData.bossHP;
            bossHP.HP = 0.5f * bossHP.maxHP;
        }

        public override void StartPhase()
        {
            base.StartPhase();
            dummyBossData.bossHP.SetMinHpPercentage(0);
        }

        DummyBossEncounter dummyBossData;
    }

    protected override void Start()
    {
        encounterStarted = true;
        bossPhases = new List<BossPhase>() {
            new DummyBossPhaseWelcome(this),
            new DummyBossEnrage(this, 0.5f, 1),
            new DummyBossEnrage(this, 0, 4)
        };
        base.Start();
    }

    protected override void EncounterSuccess()
    {
        Instantiate(butterflyPack, transform.position, Quaternion.identity);
        bossHP.Damage(null, 999999, ignoreInvulurability: true);
        base.EncounterSuccess();
    }
}
