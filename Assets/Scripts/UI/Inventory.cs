using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<SkillBase> Items;

    [SerializeField]
    private Transform ItemsContainer;

    [SerializeField]
    private Transform DraggingParent;

    [SerializeField]
    private GameObject cell;

    [SerializeField]
    private Sprite ActiveFrame;

    [SerializeField]
    private List<SkillBase> equipedSkills;

    public void Start()
    {
        Items.Clear();
        var player = GameObject.FindGameObjectWithTag("Player");
        var skills = player.GetComponent<SkillManager>();
        skills.InventoryActiveSkills.ForEach(skill => Items.Add(skill));
        skills.InventoryWeaponSkill.ForEach(skill => Items.Add(skill));
        skills.ActiveSkills.ForEach(skill => equipedSkills.Add(skill.skill));
        skills.EquippedWeapons.ForEach(weapon => equipedSkills.Add(weapon.logic));
        Render(Items, ItemsContainer, false);
        Render(equipedSkills, ItemsContainer, true);
    }

    private void Render(List<SkillBase> items, Transform conteiner, bool isActive)
    {
        int k = 0;
        for(int i = 0;i < conteiner.childCount; i++)
        {
            var empCell = conteiner.GetChild(i);
            if (k < items.Count && empCell.childCount == 0)
            {
                if (isActive)
                    MakeFrame.Frame(empCell.gameObject, ActiveFrame);
                var inst = Instantiate(cell, empCell);
                inst.GetComponent<InventoryItemPresenter>().Init(DraggingParent);
                inst.GetComponent<InventoryItemPresenter>().Render(items[k]);
                k++;
            }
        }
    }
}
