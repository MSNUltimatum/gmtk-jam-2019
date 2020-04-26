using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : HpBar
{
    protected override void Start()
    {
        base.Start();
        playerLife = GetComponentInParent<CharacterLife>();
        playerLife.hpChangedEvent.AddListener(HealthBarChange);
    }

    protected override Vector2 GetCurrentMaxHp()
    {
        return new Vector2(playerLife.GetHp(), playerLife.GetMaxHp());
    }

    private CharacterLife playerLife;
}
