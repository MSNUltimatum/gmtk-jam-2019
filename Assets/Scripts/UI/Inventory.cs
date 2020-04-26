using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform activeItemsContainer = null;
    [SerializeField]
    private Transform weaponItemsContainer = null;
    [SerializeField]
    private Transform passiveSkillsContainer = null;
    [SerializeField]
    private Transform draggingParent = null;

    [SerializeField]
    private GameObject cellPrefab = null;

    [SerializeField]
    private GameObject passivePrefab = null;

    public void Start()
    {
        nonEquippedActiveSkills = new List<SkillBase>();
        equippedActiveSkills = new List<SkillBase>();
        nonEquippedWeaponSkills = new List<SkillBase>();
        equippedWeaponSkills = new List<SkillBase>();
        passiveSkills = new List<SkillBase>();
        var player = GameObject.FindGameObjectWithTag("Player");
        skills = player.GetComponent<SkillManager>();
        skills.InventoryActiveSkills.ForEach(skill => nonEquippedActiveSkills.Add(skill));
        skills.InventoryWeaponSkill.ForEach(skill => nonEquippedWeaponSkills.Add(skill));
        skills.ActiveSkills.ForEach(skill => equippedActiveSkills.Add(skill.skill));
        skills.EquippedWeapons.ForEach(weapon => equippedWeaponSkills.Add(weapon.logic));
        foreach(var skill in skills.skills)
        {
            if (skill is PassiveSkill)
                passiveSkills.Add(skill);
        }
        Render(nonEquippedActiveSkills, activeItemsContainer, false);
        Render(equippedActiveSkills, activeItemsContainer, true);
        Render(nonEquippedWeaponSkills, weaponItemsContainer, false);
        Render(equippedWeaponSkills, weaponItemsContainer, true);
        PassiveRender(passiveSkills, passiveSkillsContainer);
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

    private void PassiveRender(List<SkillBase> items, Transform container)
    {
        for(int i = 0;i < items.Count; i++)
        {
            var inst = Instantiate(passivePrefab, container);
            var img = inst.GetComponent<PassiveItemPresenter>();
            Debug.Log(img);
            img.Render(items[i], this);
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
            else if (equippedActiveSkill.Count == 0 && skills.ActiveSkills.Count < skills.maxEquippedActiveCount)
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
            else if (equippedWeapon.Count == 0 && skills.EquippedWeapons.Count < skills.maxEquippedWeaponCount)
            {
                skills.AddSkill(currentSkill);
                var nonActiveList = skills.InventoryWeaponSkill;
                nonActiveList.Remove(currentSkill as WeaponSkill);
                MakeFrame(cell.parent.gameObject, ActiveFrame);
            }
        }
    }

    public static void MakeFrame(GameObject cell, Color frame)
    {
        cell.GetComponent<Image>().color = frame;
    }

    private List<SkillBase> nonEquippedActiveSkills = null;
    private List<SkillBase> equippedActiveSkills = null;
    private List<SkillBase> nonEquippedWeaponSkills = null;
    private List<SkillBase> equippedWeaponSkills = null;
    private List<SkillBase> passiveSkills = null;
    private SkillManager skills = null;
    private Color ActiveFrame = Color.white;
    private Color BaseFrame = Color.clear;
}
