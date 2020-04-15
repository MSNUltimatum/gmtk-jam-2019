using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossEncounter : MonoBehaviour
{
    public abstract class BossPhase
    {
        public abstract class Attack
        {
            protected float attackLength = 5f;
            public bool allowInterruption = true;
            public bool ended = true;

            public virtual void AttackStart() { }
           
            public virtual void AttackUpdate() { }

            public virtual void AttackEnd() { }

            public virtual void AttackInterrupt()
            {
                AttackEnd();
            }
        }

        public enum PhaseType { Unknown, TimeBased, HpBased, TimeOrHpBased, Trigger }
        public enum AttackOrder { Random, RandomRepeatable, Sequence, SequenceWithLoop }

        protected string phaseName = "Unnamed Phase";
        protected float endHpPercentage = -1;
        protected float phaseLength = -1;
        protected PhaseType phaseType = PhaseType.Unknown;
        protected AttackOrder attackOrder;
        protected float musicTimestamp = -1;
        protected List<Attack> attacks = null;
        protected bool phaseEnded = false;

        public BossPhase(string phaseName, PhaseType phaseType, List<Attack> attacks, AttackOrder attackOrder, 
                         float endHpPercentage = -1, float phaseLength = -1, float musicTimestamp = -1)
        {
            this.phaseName = phaseName;
            this.endHpPercentage = endHpPercentage;
            this.phaseLength = phaseLength;
            this.phaseType = phaseType;
            this.musicTimestamp = musicTimestamp;
            this.attacks = attacks;
            this.attackOrder = attackOrder;
        }

        protected int currentAttackNumber = 0;

        public void PhaseBaseUpdate() {
            if (phaseEnded) return;

            PhaseUpdate();
            phaseTimer += Time.deltaTime;

            attacks[currentAttackNumber].AttackUpdate();
            if (attacks[currentAttackNumber].ended)
            {
                StartNextAttack();
            }
        }

        protected virtual void PhaseUpdate() { }

        private void StartNextAttack()
        {
            int nextAttackNumber = -1;
            switch (attackOrder)
            {
                case AttackOrder.Random:
                    var exitCounter = 0;
                    nextAttackNumber = Random.Range(0, attacks.Count);
                    while (nextAttackNumber == currentAttackNumber && exitCounter != 50)
                    {
                        nextAttackNumber = Random.Range(0, attacks.Count);
                        exitCounter++;
                    }
                    break;
                case AttackOrder.RandomRepeatable:
                    nextAttackNumber = Random.Range(0, attacks.Count);
                    break;
                case AttackOrder.Sequence:
                    nextAttackNumber = currentAttackNumber + 1;
                    if (nextAttackNumber == attacks.Count)
                    {
                        phaseEnded = true;
                    }
                    break;
                case AttackOrder.SequenceWithLoop:
                    nextAttackNumber = (currentAttackNumber + 1) % attacks.Count;
                    break;
                default:
                    break;
            }
            attacks[currentAttackNumber].ended = false;
            currentAttackNumber = nextAttackNumber;
            attacks[currentAttackNumber].AttackStart();

            OnNextAttackStart();
        }

        protected virtual void OnNextAttackStart() { }

        public bool HasPhaseEnded()
        {
            return phaseEnded;
        }

        protected float phaseTimer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPhase = bossPhases[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (encounterOver) return;

        if (!currentPhase.HasPhaseEnded())
        {
            phaseID++;
            if (phaseID == bossPhases.Count)
            {
                encounterOver = true;
                EncounterSuccess();
            }
            else
            {
                currentPhase = bossPhases[phaseID];
            }
        }
        else
        {
            bossPhases[phaseID].PhaseBaseUpdate();
        }


        EncounterUpdate();
    }

    protected virtual void EncounterUpdate() { }
    protected virtual void EncounterSuccess() { }

    private List<BossPhase> bossPhases = new List<BossPhase>();
    private BossPhase currentPhase;
    private int phaseID = 0;
    private bool encounterOver = false;

}
