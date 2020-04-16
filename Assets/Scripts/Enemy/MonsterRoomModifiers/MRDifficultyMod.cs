using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnknownDifficultyMod", menuName = "ScriptableObject/MRMods/DifficultyMod", order = 1)]
public class MRDifficultyMod : MonsterRoomModifier
{
    [SerializeField] private float speedModifierIncrease = 1.2f;
    [SerializeField] private float healthModifier = 1.5f;
    [SerializeField] private float attackSpeedModifier = 1.2f;

    public override void ApplyModifier(MonsterLife monster)
    {
        base.ApplyModifier(monster);
        SpeedModifier(monster);
        HealthModifier(monster);
        AttackSpeedModifier(monster);
    }

    private void SpeedModifier(MonsterLife monster)
    {
        var aiAgent = monster.GetComponent<AIAgent>();
        if (aiAgent)
        {
            aiAgent.moveSpeedMult *= speedModifierIncrease;
            aiAgent.maxRotation *= speedModifierIncrease;
        }
    }

    private void HealthModifier(MonsterLife monster)
    {
        monster.maxHP *= healthModifier;
        monster.HP *= healthModifier;
    }

    private void AttackSpeedModifier(MonsterLife monster)
    {
        var attacks = monster.GetComponentsInChildren<Attack>();
        foreach (var attack in attacks)
        {
            attack.attackSpeedModifier *= attackSpeedModifier;
        }
    }
}
