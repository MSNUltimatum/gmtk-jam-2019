using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseCursorObj = null;

    public bool shotFrame = false; //flag for reactions on shot
    public void LoadNewWeapon(SkillManager.EquippedWeapon weapon, float punishmentReload)
    {
        currentWeapon = weapon;
        reloadTimeLeft = punishmentReload;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        GameObject.Instantiate(mouseCursorObj);
        skillManager = GetComponent<SkillManager>();
    }

    private void Update()
    {
        if (Pause.Paused)
        {
            Cursor.visible = true;
            return;
        }
        Cursor.visible = false;

        shotFrame = false;
        if (reloadTimeLeft > 0)
        {
            reloadTimeLeft -= Time.deltaTime;
        }
        else if (Input.GetButton("Fire1"))
        {
            currentWeapon.reloadTimeLeft = 0;
            Vector3 mousePos = Input.mousePosition;
            var screenPoint = mainCamera.WorldToScreenPoint(transform.localPosition);
            var ammoNeeded = currentWeapon.logic.AmmoConsumption();
            if (currentWeapon.ammoLeft >= ammoNeeded)
            {
                currentWeapon.ammoLeft -= ammoNeeded;
                currentWeapon.logic.Attack(this, mousePos, screenPoint);
                shotFrame = true;
            }
            reloadTimeLeft = currentWeapon.logic.timeBetweenAttacks;
            if (currentWeapon.ammoLeft == 0)
            {
                skillManager.ReloadWeaponIfNeeded();
                reloadTimeLeft = 1f; // WARNING: MAGIC CONSTANT TO PREVENT PLAYER FROM SHOOTING WHEN HE STARTED RELOADING
            }
        }
        if (Input.GetKeyDown(reloadButton))
        {
            skillManager.ReloadWeaponIfNeeded();
        }
    }

    private float reloadTimeLeft = 0;
    private Camera mainCamera;
    private KeyCode reloadButton = KeyCode.R;
    private SkillManager skillManager;
    public SkillManager.EquippedWeapon currentWeapon;
}
