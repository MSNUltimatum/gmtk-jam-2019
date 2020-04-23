using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedMod", menuName = "ScriptableObject/PassiveSkill/AttackSpeedMod", order = 1)]
public class AttackSpeedPassiveSkill : PassiveSkill
{
    [SerializeField] private float addValue = 0.1f;

    public override void InitializeSkill()
    {
        base.InitializeSkill();
        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CharacterShooting>()
            .AddToAttackSpeed(addValue);
    }
}
