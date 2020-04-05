using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform itemsContainer = null;
    [SerializeField]
    private Transform draggingParent = null;

    [SerializeField]
    private GameObject cellPrefab = null;

    [SerializeField]
    private Sprite ActiveFrame = null;
    [SerializeField]
    public Sprite BaseFrame = null;

    public void Start()
    {
        nonEquippedSkills.Clear();
        equippedSkills.Clear();
        var player = GameObject.FindGameObjectWithTag("Player");
        skills = player.GetComponent<SkillManager>();
        skills.InventoryActiveSkills.ForEach(skill => nonEquippedSkills.Add(skill));
        skills.InventoryWeaponSkill.ForEach(skill => nonEquippedSkills.Add(skill));
        skills.ActiveSkills.ForEach(skill => equippedSkills.Add(skill.skill));
        skills.EquippedWeapons.ForEach(weapon => equippedSkills.Add(weapon.logic));
        Render(equippedSkills, itemsContainer, true);
        Render(nonEquippedSkills, itemsContainer, false);
    }

    private void Render(List<SkillBase> items, Transform container, bool isActive)
    {
        int k = 0;
        for(int i = 0;i < container.childCount; i++)
        {
            var empCell = container.GetChild(i);
            if (k < items.Count && empCell.childCount == 0)
            {
                if (isActive)
                    MakeFrame(empCell.gameObject, ActiveFrame);
                var inst = Instantiate(cellPrefab, empCell);
                var skillImage = inst.GetComponent<InventoryItemPresenter>();
                skillImage.Init(draggingParent);
                skillImage.Render(items[k], this);
                k++;
            }
        }
    }

    public void OnCellClick(SkillBase currentSkill, Transform cell)
    {
        //если скил активный и он экипирован
        if (currentSkill is ActiveSkill) 
        {
            var equippedActiveSkill = skills.ActiveSkills.FindAll(skill => skill.skill == currentSkill)[0];
            if (equippedActiveSkill != null)
            {
                if (equippedActiveSkill.cooldown == 0)
                {
                    skills.ActiveSkills.RemoveAll(skill => skill.skill == currentSkill);
                    var nonActiveList = skills.InventoryActiveSkills;
                    nonActiveList.Add(currentSkill as ActiveSkill);
                    MakeFrame(cell.parent.gameObject, BaseFrame);
                    skills.ApplySkillSprites();
                }
            }
            else if (skills.ActiveSkills.Count < skills.equippedActiveCount)
            {
                var activeList = skills.ActiveSkills;
                activeList.Add(new SkillManager.EquippedActiveSkill(currentSkill as ActiveSkill));
                skills.ActiveSkills = activeList;
                var nonActiveList = skills.InventoryActiveSkills;
                nonActiveList.Remove(currentSkill as ActiveSkill);
                MakeFrame(cell.parent.gameObject, ActiveFrame);
                skills.ApplySkillSprites();
            }
        }
        else if (currentSkill is WeaponSkill && skills.EquippedWeapons.Where(weapon => weapon.logic == currentSkill).Count() > 0)
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
                MakeFrame(cell.parent.gameObject, BaseFrame);
            }
        }
        else if (currentSkill is WeaponSkill && skills.EquippedWeapons.Count < skills.equippedWeaponCount)
        {
            var activeList = skills.EquippedWeapons;
            activeList.Add(new SkillManager.EquippedWeapon(currentSkill as WeaponSkill, activeList.Count));
            skills.EquippedWeapons = activeList;
            var nonActiveList = skills.InventoryWeaponSkill;
            nonActiveList.Remove(currentSkill as WeaponSkill);
            MakeFrame(cell.parent.gameObject, ActiveFrame);
            skills.ApplyWeaponSprites();
        }
    }

    public static void MakeFrame(GameObject cell, Sprite frame)
    {
        cell.GetComponent<Image>().sprite = frame;
    }

    private List<SkillBase> nonEquippedSkills = null;
    private List<SkillBase> equippedSkills = null;
    private SkillManager skills = null;
}
