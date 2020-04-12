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
        nonEquippedSkills = new List<SkillBase>();
        equippedSkills = new List<SkillBase>();
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
            var equippedActiveSkill = skills.ActiveSkills.FindAll(skill => skill.skill == currentSkill);
            if (equippedActiveSkill.Count != 0 && equippedActiveSkill[0].cooldown == 0)
            {
                skills.ActiveSkills.RemoveAll(skill => skill.skill == currentSkill);
                var nonActiveList = skills.InventoryActiveSkills;
                nonActiveList.Add(currentSkill as ActiveSkill);
                MakeFrame(cell.parent.gameObject, BaseFrame);
                skills.RefreshUI();
            }
            else if (skills.ActiveSkills.Count < skills.maxEquippedActiveCount)
            {
                skills.AddSkill(currentSkill);
                var nonActiveList = skills.InventoryActiveSkills;
                nonActiveList.Remove(currentSkill as ActiveSkill);
                MakeFrame(cell.parent.gameObject, ActiveFrame);
            }
        }
        else if (currentSkill is WeaponSkill)
        {
            var equippedWeapon = skills.EquippedWeapons.FindAll(skill => skill.logic == currentSkill);
            if (equippedWeapon.Count != 0 && equippedWeapon[0].reloadTimeLeft == 0)
            {
                List<SkillManager.EquippedWeapon> tmpList = new List<SkillManager.EquippedWeapon>();
                skills.EquippedWeapons.RemoveAll(skill => skill.logic == currentSkill);
                skills.EquippedWeapons.ForEach(skill => tmpList.Add(skill));
                skills.ClearWeapons();
                if (tmpList.Count > 0) tmpList.ForEach(skill => skills.AddSkill(skill.logic));
                else skills.RefreshUI();
                var nonActiveList = skills.InventoryWeaponSkill;
                nonActiveList.Add(currentSkill as WeaponSkill);
                MakeFrame(cell.parent.gameObject, BaseFrame);
            }
            else if (skills.EquippedWeapons.Count < skills.maxEquippedWeaponCount)
            {
                skills.AddSkill(currentSkill);
                var nonActiveList = skills.InventoryWeaponSkill;
                nonActiveList.Remove(currentSkill as WeaponSkill);
                MakeFrame(cell.parent.gameObject, ActiveFrame);
            }
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
