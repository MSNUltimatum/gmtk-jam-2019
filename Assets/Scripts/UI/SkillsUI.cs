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

    public static int weaponsCount = 5;
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

    public void UpdateReloadVisualCooldown(float[] proportionOfTimeLeft)
    {
        var debugSum = 0f;
        for (int i = 0; i < weaponsCount; i++)
        {
            weaponCooldownEffect[i].SetFloat("_CooldownProgress", proportionOfTimeLeft[i]);
            debugSum += proportionOfTimeLeft[i];
        }
        Debug.Log(debugSum);
    }
}
