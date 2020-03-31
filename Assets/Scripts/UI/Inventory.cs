using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<SkillBase> Items = null;

    [SerializeField]
    private Transform ItemsContainer = null;

    [SerializeField]
    private Transform DraggingParent = null;

    [SerializeField]
    private GameObject cell = null;

    [SerializeField]
    private Sprite ActiveFrame = null;

    [SerializeField]
    public Sprite BaseFrame = null;

    [SerializeField]
    private List<SkillBase> equipedSkills = null;

    private SkillManager skills = null;

    public void Start()
    {
        Items.Clear();
        var player = GameObject.FindGameObjectWithTag("Player");
        skills = player.GetComponent<SkillManager>();
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
                inst.GetComponent<InventoryItemPresenter>().Render(items[k], this);
                k++;
            }
        }
    }

    public void OnCellClick(SkillBase currentSkill, Transform cell)
    {
        if (currentSkill is ActiveSkill && skills.ActiveSkills.Where(skill => skill.skill == currentSkill).ToArray().Length > 0)
        {
            var activeList = skills.ActiveSkills;
            if (activeList.FindAll(skill => skill.skill == currentSkill)[0].cooldown == 0)
            {
                activeList.RemoveAll(skill => skill.skill == currentSkill);
                skills.ActiveSkills = activeList;
                var nonActiveList = skills.InventoryActiveSkills;
                nonActiveList.Add(currentSkill as ActiveSkill);
                MakeFrame.Frame(cell.parent.gameObject, BaseFrame);
                skills.ApplySkillSprites();
            }
        }
        else if (currentSkill is WeaponSkill && skills.EquippedWeapons.Where(weapon => weapon.logic == currentSkill).ToArray().Length > 0)
        {
            var activeList = skills.EquippedWeapons;
            List<SkillManager.EquippedWeapon> tmpList = new List<SkillManager.EquippedWeapon>();
            if (activeList.FindAll(skill => skill.logic == currentSkill)[0].reloadTimeLeft == 0)
            {
                activeList.RemoveAll(skill => skill.logic == currentSkill);
                activeList.ForEach(skill => tmpList.Add(skill));
                skills.ClearWeapons();
                if (tmpList.Count > 0)
                    tmpList.ForEach(skill => skills.AddSkill(skill.logic));
                var nonActiveList = skills.InventoryWeaponSkill;
                nonActiveList.Add(currentSkill as WeaponSkill);
                MakeFrame.Frame(cell.parent.gameObject, BaseFrame);
            }
        }
        else if (currentSkill is ActiveSkill && skills.ActiveSkills.Count < skills.equippedActiveCount)
        {
            var activeList = skills.ActiveSkills;
            activeList.Add(new SkillManager.EquippedActiveSkill(currentSkill as ActiveSkill));
            skills.ActiveSkills = activeList;
            var nonActiveList = skills.InventoryActiveSkills;
            nonActiveList.Remove(currentSkill as ActiveSkill);
            MakeFrame.Frame(cell.parent.gameObject, ActiveFrame);
            skills.ApplySkillSprites();
        }
        else if (currentSkill is WeaponSkill && skills.EquippedWeapons.Count < skills.equippedWeaponCount)
        {
            var activeList = skills.EquippedWeapons;
            activeList.Add(new SkillManager.EquippedWeapon(currentSkill as WeaponSkill, activeList.Count));
            skills.EquippedWeapons = activeList;
            var nonActiveList = skills.InventoryWeaponSkill;
            nonActiveList.Remove(currentSkill as WeaponSkill);
            MakeFrame.Frame(cell.parent.gameObject, ActiveFrame);
            skills.ApplyWeaponSprites();
        }
    }
}
