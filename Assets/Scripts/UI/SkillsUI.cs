using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUI : MonoBehaviour
{
    private void Awake()
    {
        InitializeWeaponUI();
        InitializeSkillUI();
    }

    [SerializeField]
    private GameObject weaponContainerUI = null;

    public static int weaponsCount = 3;
    public Transform[] weaponCells = new Transform[weaponsCount];
    private Material[] weaponCooldownEffect = new Material[weaponsCount];

    [SerializeField]
    private GameObject skillContainerUI = null;
    public static int skillCount = 5;
    private Image[] skillImage = new Image[skillCount];
    private Material[] skillCooldownEffectCells = new Material[skillCount];

    private void InitializeWeaponUI()
    {
        weaponContainerUI.SetActive(true);

        for (int i = 0; i < weaponsCount; i++)
        {
            var backgroundReload = weaponCells[i].GetChild(0).GetComponent<Image>();
            backgroundReload.material = new Material(backgroundReload.material);
            weaponCooldownEffect[i] = backgroundReload.material;
        }
    }

    private void InitializeSkillUI()
    {
        skillContainerUI.SetActive(true);

        for (int i = 0; i < skillCount; i++)
        {
            var skillCell = skillContainerUI.transform.GetChild(i);
            skillImage[i] = skillCell.transform.GetChild(1).GetComponent<Image>();
            var backgroundReload = skillCell.GetChild(0).GetComponent<Image>();
            var backgroundReloadmaterial = new Material(backgroundReload.material);
            backgroundReload.material = backgroundReloadmaterial;
            skillCooldownEffectCells[i] = backgroundReload.material;
        }
    }

    public void UpdateSkillRecoverVisualCooldown(float[] proportionOfTimeLeft)
    {
        for (int i = 0; i < skillCount; i++)
        {
            skillCooldownEffectCells[i].SetFloat("_CooldownProgress", proportionOfTimeLeft[i]);
        }
    }

    public void SetSkillSprites(Sprite[] skillSprites)
    {
        for (int i = 0; i < skillCount; i++)
        {
            if (skillSprites[i] != null)
            {
                var thisSkillImage = skillImage[i];
                thisSkillImage.color = Color.white;
                thisSkillImage.sprite = skillSprites[i];
                skillImage[i] = thisSkillImage;
            }
            else
            {
                skillImage[i].color = Color.clear;
            }
        }
    }

    public void UpdateWeaponReloadVisualCooldown(float[] proportionOfTimeLeft, int currentWeaponIndex)
    {
        var diffInCellNumeration = 1 - currentWeaponIndex;
        for (int i = 0; i < weaponsCount; i++)
        {
            var cellIndex = (weaponsCount + i + diffInCellNumeration) % weaponsCount;
            weaponCooldownEffect[cellIndex].SetFloat("_CooldownProgress", proportionOfTimeLeft[i]);
        }
    }

    public void SetWeaponSprites(Sprite[] weaponSprites, int currentWeaponIndex)
    {
        var diffInCellNumeration = 1 - currentWeaponIndex;
        for (int i = 0; i < weaponsCount; i++)
        {
            var cellIndex = (weaponsCount + i + diffInCellNumeration) % weaponsCount;
            if (weaponSprites[i] != null)
            {
                var weaponImage = weaponCells[cellIndex].GetChild(1).GetComponent<Image>();
                weaponImage.color = Color.white;
                weaponImage.sprite = weaponSprites[i];
            }
            else
            {
                weaponCells[cellIndex].GetChild(1).GetComponent<Image>().color = Color.clear;
            }
        }
    }
}
