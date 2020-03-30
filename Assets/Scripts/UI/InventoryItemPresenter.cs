using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class InventoryItemPresenter : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    public Image itemImage;

    [SerializeField]
    public GameObject emptyCell;

    [SerializeField]
    private Sprite BaseFrame;

    [SerializeField]
    private Sprite ActiveFrame;

    [SerializeField]
    private Sprite baseImg;

    private SkillBase currentSkill;

    private Transform draggingParent;
    private Transform originalParent;


    public void Init(Transform draggingparent)
    {
        draggingParent = draggingparent;
        originalParent = transform.parent;
        Reboot();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(draggingParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       int closestInex = 0;
       for (int i = 0; i < originalParent.parent.transform.childCount; i++)
       {
           if (Vector3.Distance(transform.position, originalParent.parent.transform.GetChild(i).position) <
               Vector3.Distance(transform.position, originalParent.parent.transform.GetChild(closestInex).position))
           {
               closestInex = i;
           }
       }
        if (originalParent.parent.GetChild(closestInex).childCount == 0)
        {
            transform.SetParent(originalParent.parent.GetChild(closestInex));
            transform.localPosition = new Vector2(0, 0);
        }
        else
        {
            var cell = originalParent.parent.GetChild(closestInex).GetChild(0);
            cell.SetParent(originalParent);
            cell.transform.localPosition = new Vector2(0, 0);
            cell.GetComponent<InventoryItemPresenter>().SetOriginalParent(originalParent);
            transform.SetParent(originalParent.parent.GetChild(closestInex));
            transform.localPosition = new Vector2(0, 0);
        }
        originalParent = transform.parent;
    }

    public void Render(SkillBase item)
    {
        itemImage.sprite = item.pickupSprite;
        Image img = GetComponent<Image>();
        var tmp = img.color;
        currentSkill = item;
        //GetComponent<Button>(). = OnCellClick();
    }

    private void Reboot()
    {
        itemImage.sprite = baseImg;
    }

    public void SetOriginalParent(Transform parent)
    {
        originalParent = parent;
    }

    public void OnCellClick()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var skills = player.GetComponent<SkillManager>();
        if (currentSkill is ActiveSkill && skills.ActiveSkills.Where(skill => skill.skill == currentSkill).ToArray().Length > 0)
        {
            var activeList = skills.ActiveSkills;
            if (activeList.FindAll(skill => skill.skill == currentSkill)[0].cooldown == 0)
            {
                activeList.RemoveAll(skill => skill.skill == currentSkill);
                skills.ActiveSkills = activeList;
                var nonActiveList = skills.InventoryActiveSkills;
                nonActiveList.Add(currentSkill as ActiveSkill);
                MakeFrame.Frame(transform.parent.gameObject, BaseFrame);
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
                tmpList.ForEach(skill => skills.AddSkill(skill.logic));
                var nonActiveList = skills.InventoryWeaponSkill;
                nonActiveList.Add(currentSkill as WeaponSkill);
                MakeFrame.Frame(transform.parent.gameObject, BaseFrame);
               // skills.ApplyWeaponSprites();
            }
        }
        else if(currentSkill is ActiveSkill && skills.ActiveSkills.Count < skills.equippedActiveCount)
        {
            var activeList = skills.ActiveSkills;
            activeList.Add(new SkillManager.EquippedActiveSkill(currentSkill as ActiveSkill));
            skills.ActiveSkills = activeList;
            var nonActiveList = skills.InventoryActiveSkills;
            nonActiveList.Remove(currentSkill as ActiveSkill);
            MakeFrame.Frame(transform.parent.gameObject, ActiveFrame);
            skills.ApplySkillSprites();
        }
        else if(currentSkill is WeaponSkill && skills.EquippedWeapons.Count < skills.equippedWeaponCount)
        {
            var activeList = skills.EquippedWeapons;
            activeList.Add(new SkillManager.EquippedWeapon(currentSkill as WeaponSkill, activeList.Count));
            skills.EquippedWeapons = activeList;
            var nonActiveList = skills.InventoryWeaponSkill;
            nonActiveList.Remove(currentSkill as WeaponSkill);
            MakeFrame.Frame(transform.parent.gameObject, ActiveFrame);
            skills.ApplyWeaponSprites();
        }
    }
}
