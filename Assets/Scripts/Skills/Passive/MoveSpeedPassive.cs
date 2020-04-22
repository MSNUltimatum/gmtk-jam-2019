using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveSpeedUpgrade", menuName = "ScriptableObject/PassiveSkill/MoveSpeedUpgrade", order = 1)]
public class MoveSpeedPassive : PassiveSkill
{
    [SerializeField] private float moveSpeedAmplifier = 0.25f;
    [SerializeField] private GameObject VFXTrail = null;

    public override void InitializeSkill()
    {
        base.InitializeSkill();
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterMovement>().AddToSpeedMultiplier(moveSpeedAmplifier);
        Instantiate(VFXTrail, player.transform.position - player.transform.up * 0.4f, Quaternion.identity, player.transform);
    }
}
