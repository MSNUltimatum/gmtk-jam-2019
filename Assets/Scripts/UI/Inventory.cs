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
    public GameObject cellPrefab = null;
    [SerializeField]
    private GameObject passivePrefab = null;

    public void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        skills = player.GetComponent<SkillManager>();
        nonEquippedWeaponSkills = new List<SkillBase>();
        equippedWeaponSkills = new List<SkillBase>();
        nonEquippedActiveSkills = new List<SkillBase>();
        equippedActiveSkills = new List<SkillBase>();
        passiveSkills = new List<SkillBase>();
        makecontainer(activeItemsContainer);
        makecontainer(weaponItemsContainer);
        makecontainer(passiveSkillsContainer);
        addActiveSkills();
        addWeaponSkills();
        addPassiveSkills();
        isStarted = true;
    }

    public void addSkill(SkillBase skill)
    {
        if(skill is ActiveSkill)
        {
            rebootContainer(activeItemsContainer);
            addActiveSkills();
        }
        else if(skill is PassiveSkill)
        {
            rebootContainer(passiveSkillsContainer);
            addPassiveSkills();
        }
        else if(skill is WeaponSkill) 
        {
            rebootContainer(weaponItemsContainer);
            addWeaponSkills();
        }
    }

    private void addActiveSkills()
    {
        if (nonEquippedActiveSkills != null)
            nonEquippedActiveSkills.Clear();
        if (equippedActiveSkills != null)
            equippedActiveSkills.Clear();
        skills.InventoryActiveSkills.ForEach(skill => nonEquippedActiveSkills.Add(skill));
        skills.ActiveSkills.ForEach(skill => equippedActiveSkills.Add(skill.skill));
        Render(nonEquippedActiveSkills, activeItemsContainer, false);
        Render(equippedActiveSkills, activeItemsContainer, true);
    }

    private void addWeaponSkills()
    {
        if (nonEquippedWeaponSkills.Count > 0)
            nonEquippedWeaponSkills.Clear();
        if (equippedWeaponSkills.Count > 0)
            equippedWeaponSkills.Clear();

        skills.InventoryWeaponSkill.ForEach(skill => nonEquippedWeaponSkills.Add(skill));
        skills.EquippedWeapons.ForEach(weapon => equippedWeaponSkills.Add(weapon.logic));
        Render(nonEquippedWeaponSkills, weaponItemsContainer, false);
        Render(equippedWeaponSkills, weaponItemsContainer, true);
    }

    private void addPassiveSkills()
    {
        foreach (var skill in skills.skills)
        {
            if (skill is PassiveSkill)
                passiveSkills.Add(skill);
        }
        PassiveRender(passiveSkills, passiveSkillsContainer);
    }

    private void rebootContainer(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            var cell = container.GetChild(i);
            if (cell.childCount > 0)
            {
                for (int j = 0; j < cell.childCount; j++)
                    cell.GetChild(0).GetComponent<Image>().sprite = cellPrefab.GetComponent<Image>().sprite;
            }
            MakeFrame(cell.gameObject, BaseFrame);
        }
    }

    private void Render(List<SkillBase> items, Transform container, bool isActive)
    {
        int k = 0;
        for(int i = 0; i < container.childCount; i++)
        {
            var empCell = container.GetChild(i);
            if (k < items.Count && empCell.GetChild(0).GetComponent<Image>().sprite == cellPrefab.GetComponent<Image>().sprite)
            {
                if (isActive)
                    MakeFrame(empCell.gameObject, ActiveFrame);
                var skillImage = empCell.GetChild(0).GetComponent<InventoryItemPresenter>();
                skillImage.Init(draggingParent);
                skillImage.Render(items[k], this);
                k++;
            }
        }
    }

    private void makecontainer(Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            var empCell = container.GetChild(i);
            MakeFrame(empCell.gameObject, BaseFrame);
            var inst = Instantiate(cellPrefab, empCell);
        }
    }

    private void PassiveRender(List<SkillBase> items, Transform container)
    {
        for(int i = 0;i < items.Count; i++)
        {
            var inst = Instantiate(passivePrefab, container);
            var img = inst.GetComponent<PassiveItemPresenter>();
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
    public bool isStarted = false;
}
