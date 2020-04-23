using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyForTutorial : WeaponSkill
{
    [SerializeField] private GameObject keyPrefab;

    public override int AmmoConsumption()
    {
        return 0;
    }

    public override void Attack(CharacterShooting attackManager, Vector3 mousePos)
    {
        base.Attack(attackManager, mousePos);
        if (!opened)
        {
            GameObject.Find("TutorialDoor");
        }
    }

    private bool opened = false;
    private GameObject keyInstance;
}
