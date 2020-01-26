using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUI : MonoBehaviour
{
    private void Awake()
    {
        InitializeWeaponUI();
    }

    [SerializeField]
    private GameObject weaponContainerUI;

    public static int weaponsCount = 3;
    public Transform[] weaponCells = new Transform[weaponsCount];
    private Material[] weaponCooldownEffect = new Material[weaponsCount];

    private float centerWeaponIconAlpha = 0.625f;
    private float borderWeaponIconAlpha = 0.325f;

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

    public void UpdateReloadVisualCooldown(float[] proportionOfTimeLeft, int currentWeaponIndex)
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
