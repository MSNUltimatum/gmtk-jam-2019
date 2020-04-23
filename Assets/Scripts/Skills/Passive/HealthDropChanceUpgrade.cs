using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasedHPDropChance", menuName = "ScriptableObject/PassiveSkill/IncreasedHPDropChance", order = 1)]
public class HealthDropChanceUpgrade : PassiveSkill
{
    [SerializeField] float DropChanceMultiplier = 0.25f;

    public override void InitializeSkill()
    {
        base.InitializeSkill();
        GameObject.FindGameObjectWithTag("Player")
            .GetComponent<CharacterLife>()
            .AddToHPDropChanceAmp(DropChanceMultiplier);
    }
}
