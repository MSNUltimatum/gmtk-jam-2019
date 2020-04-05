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
        timeBetweenAttacks = punishmentReload;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        cameraShaker = mainCamera.GetComponent<CameraShaker>();
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
        
        if (timeBetweenAttacks > 0)
        {
            timeBetweenAttacks -= Time.deltaTime;
        }

        if (currentWeapon == null) return;
        else if (Input.GetButton("Fire1"))
        {
            
            Vector3 mousePos = Input.mousePosition;
            var screenPoint = mainCamera.WorldToScreenPoint(transform.localPosition);
            var ammoNeeded = currentWeapon.logic.AmmoConsumption();
            if (currentWeapon.ammoLeft >= ammoNeeded)
            {
                currentWeapon.reloadTimeLeft = 0;
                currentWeapon.ammoLeft -= ammoNeeded;
                currentWeapon.logic.Attack(this, mousePos, screenPoint);
                cameraShaker.ShakeCamera(0.25f);
                shotFrame = true;
            }
            timeBetweenAttacks = currentWeapon.logic.timeBetweenAttacks;
            if (currentWeapon.ammoLeft == 0)
            {
                skillManager.ReloadWeaponIfNeeded();
                timeBetweenAttacks = 1f; // WARNING: MAGIC CONSTANT TO PREVENT PLAYER FROM FIRING WHEN HE STARTED RELOADING
            }
        }
        if (Input.GetKeyDown(reloadButton))
        {
            skillManager.ReloadWeaponIfNeeded();
        }
    }

    private float timeBetweenAttacks = 0;

    private Camera mainCamera;
    private CameraShaker cameraShaker;

    private KeyCode reloadButton = KeyCode.R;
    private SkillManager skillManager;
    public SkillManager.EquippedWeapon currentWeapon;
}
