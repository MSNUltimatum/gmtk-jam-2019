using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SkillManager : MonoBehaviour
{
    public EquippedWeapon equippedWeapon = null;

    [SerializeField, Header("Important")]
    private bool forceSkillRewrite = false;

    #region Skill Register & Load
    private Dictionary<string, SkillBase> registeredSkills = new Dictionary<string, SkillBase>();

    [SerializeField, Tooltip("Skill database-like prefab")]
    private GameObject prefabSkillLoader = null;
    /// <summary>
    /// Get all skills in-game from database object
    /// </summary>
    public void FillRegisteredSkills()
    {
        if (prefabSkillLoader == null)
        {
            Debug.LogError("Skill loader prefab not assigned! Can't load skills because of that");
        }
        else
        {
            var skillContainer = prefabSkillLoader.GetComponent<SkillPullFromDatabase>();
            if (skillContainer != null)
            {
                foreach (var skill in skillContainer.LoadSkills().Values)
                {
                    registeredSkills.Add(skill.SkillName(), Instantiate(skill));
                }
            }
            else
            {
                Debug.LogError("Skill loader has no database-pull-script assigned! Can't load skills because of that");
            }   
        }
    }

    public bool SaveSkill(string name, SkillBase skill)
    {
        if (!registeredSkills.ContainsKey(name))
        {
            registeredSkills.Add(name, skill);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PrintRegisteredSkills()
    {
        print($"Skills registered: {registeredSkills.Count}");
        foreach (var skill in registeredSkills.Keys)
        {
            print(skill + " " + registeredSkills[skill]);
        }
    }

    public SkillBase LoadSkill(string name)
    {
        //print(name);
        return registeredSkills[name];
    }

    private string fileName = "progress.bin";

    private void SaveSkills()
    {
        BinaryFormatter binaryformatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + fileName);
        var skillsSavedInfo = new SkillsRecord(skills);
        foreach (var skill in skillsSavedInfo.weapons)
            binaryformatter.Serialize(file, skillsSavedInfo);

        file.Close();
    }

    /// <summary>
    /// Loads skills by name. Grab skill information from "registered" skills
    /// </summary>
    private void LoadSkills()
    {
        if (!forceSkillRewrite && File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter binaryformatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            var skillsSavedInfo = (SkillsRecord)binaryformatter.Deserialize(file);
            file.Close();

            skills = new List<SkillBase>();
            foreach (var skill in skillsSavedInfo.activeSkills)
            {
                if (!String.IsNullOrEmpty(skill)) skills.Add(registeredSkills[skill] as ActiveSkill);
            }
            foreach (var skill in skillsSavedInfo.passiveSkills)
            {
                if (!String.IsNullOrEmpty(skill)) skills.Add(registeredSkills[skill] as PassiveSkill);
            }
            foreach (var skill in skillsSavedInfo.weapons)
            {
                if (!String.IsNullOrEmpty(skill)) skills.Add(registeredSkills[skill] as WeaponSkill);
            }
        }
        else
        {
            SaveSkills();
            if (!File.Exists(Application.persistentDataPath + fileName))
            {
                Debug.LogError("Critical error: save file was not created");
            }
            else
            {
                // Warning: Possible infinite loop here!!!
                forceSkillRewrite = false;
                LoadSkills();
            }
        }
    }

    #endregion

    [Serializable]
    private class EquippedActiveSkill
    {
        public ActiveSkill skill;
        public float cooldown;
        public float activeTimeLeft;

        public EquippedActiveSkill(ActiveSkill skill)
        {
            this.skill = skill;
            cooldown = 0;
            activeTimeLeft = 0;
        }
    }

    [Serializable]
    public class EquippedWeapon
    {
        public WeaponSkill logic;
        public int ammoLeft;
        public float reloadTimeLeft;
        public float lastTimeEquipped;
        public int weaponIndex;
        public AudioClip[] attackSound;

        public EquippedWeapon(WeaponSkill weapon, int weaponIndex)
        {
            this.logic = weapon;
            ammoLeft = weapon.ammoMagazine;
            reloadTimeLeft = 0;
            this.weaponIndex = weaponIndex;
            attackSound = weapon.attackSound;
            lastTimeEquipped = Time.time;
        }
    }

    private void Awake()
    {
        RelodScene.OnSceneChange.AddListener(SaveSkills);
        skillsUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<SkillsUI>();
    }

    List<WeaponSkill> inventoryWeaponSkills = new List<WeaponSkill>();
    List<ActiveSkill> inventoryActiveSkills = new List<ActiveSkill>();

    public void AddSkill(SkillBase skill)
    {
        skills.Add(skill);
        skill.InitializeSkill();
        if (skill is ActiveSkill)
        {
            if (activeSkills.Count >= 5)
            {
                inventoryActiveSkills.Add(skill as ActiveSkill);
            }
            else
            {
                activeSkills.Add(new EquippedActiveSkill(skill as ActiveSkill));
            }
        }
        else
        {
            if (equippedWeapons.Count >= 3)
            {
                inventoryWeaponSkills.Add(skill as WeaponSkill);
            }
            else
            {
                equippedWeapons.Add(new EquippedWeapon(skill as WeaponSkill, equippedWeapons.Count));
            }

        }
        InitializeUI();
    }

    private void Start()
    {
        FillRegisteredSkills();
        //PrintRegisteredSkills();

        LoadSkills();
        InitializeSkills();
        attackManager = GetComponent<CharacterShooting>();
        if (attackManager != null)
        {
            attackManager.LoadNewWeapon(equippedWeapon, 0);
        }
    }

    private void InitializeSkills()
    {        
        foreach (var s in skills)
        {
            if (s is ActiveSkill)
            {
                if (activeSkills.Count >= 5)
                {
                    inventoryActiveSkills.Add(s as ActiveSkill);
                }
                else
                {
                    activeSkills.Add(new EquippedActiveSkill(s as ActiveSkill));
                }
            }
            else
            {
                if (equippedWeapons.Count >= 3)
                {
                    inventoryWeaponSkills.Add(s as WeaponSkill);
                }
                else
                {
                    equippedWeapons.Add(new EquippedWeapon(s as WeaponSkill, equippedWeapons.Count));
                }
            }
            s.InitializeSkill();
        }
        equippedWeapon = equippedWeapons[0];

        InitializeUI();
    }

    private List<KeyCode> keys = new List<KeyCode>() {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0
    };

    private void Update()
    {
        // Check for a key pressed for active skill
        for (int i = 0; i < activeSkills.Count; i++)
        {
            if (Input.GetKeyDown(keys[i]) && activeSkills.Count >= i && activeSkills[i].cooldown <= 0f)
            {
                activeSkills[i].skill.ActivateSkill();
                activeSkills[i].activeTimeLeft = activeSkills[i].skill.activeDuration;
                activeSkills[i].cooldown = activeSkills[i].skill.cooldownDuration;
            }
        }

        // Update effect, cooldown and active time left for active skill
        float[] skillCooldownsProportion = new float[SkillsUI.skillCount];
        bool[] isActiveSkill = new bool[SkillsUI.skillCount];
        for (int i = 0; i < activeSkills.Count; i++)
        {
            activeSkills[i].cooldown = Mathf.Max(0, activeSkills[i].cooldown - Time.deltaTime);

            if (activeSkills[i].activeTimeLeft > 0)
            {
                activeSkills[i].skill.UpdateEffect();
                activeSkills[i].activeTimeLeft = Mathf.Max(0, activeSkills[i].activeTimeLeft - Time.deltaTime);
                if (activeSkills[i].activeTimeLeft <= 0)
                {
                    activeSkills[i].skill.EndOfSkill();
                }
            }
            skillCooldownsProportion[i] = activeSkills[i].cooldown / activeSkills[i].skill.cooldownDuration;

            isActiveSkill[i] = activeSkills[i].activeTimeLeft > 0;
        }
        skillsUI.UpdateSkillRecoverVisualCooldown(skillCooldownsProportion, isActiveSkill);

        // Switch weapon
        if (Input.GetKeyDown(rotateWeaponLeft) || Input.GetKeyDown(rotateWeaponRight))
        {
            var newWeaponIndex = 0;
            if (Input.GetKeyDown(rotateWeaponLeft))
                newWeaponIndex = (equippedWeapon.weaponIndex + equippedWeapons.Count - 1) % equippedWeapons.Count;
            else if (Input.GetKeyDown(rotateWeaponRight))
                newWeaponIndex = (equippedWeapon.weaponIndex + 1) % equippedWeapons.Count;
            if (equippedWeapon.ammoLeft < equippedWeapon.logic.ammoMagazine)
            {
                ReloadWeaponIfNeeded();
            }
            equippedWeapon = equippedWeapons[newWeaponIndex];
            foreach (var weapon in equippedWeapons)
            attackManager.LoadNewWeapon(equippedWeapon, equippedWeapon.logic.timeBetweenAttacks);
            ApplyWeaponSprites();
        }


        // Update reload time of all weapons & call update
        float[] weaponCooldownsProportion = new float[SkillsUI.weaponsCount];
        int j = 0;
        foreach (var weapon in equippedWeapons)
        {
            if (weapon.reloadTimeLeft != 0)
            {
                weapon.reloadTimeLeft = Mathf.Max(0, weapon.reloadTimeLeft - Time.deltaTime);
                weapon.ammoLeft = Mathf.Max(weapon.ammoLeft, (int)Mathf.Floor(Mathf.Lerp(weapon.logic.ammoMagazine, 0, (weapon.reloadTimeLeft - 0.01f) / weapon.logic.reloadTime)));
            }
            weaponCooldownsProportion[j] = weapon.reloadTimeLeft / weapon.logic.reloadTime;

            weapon.logic.UpdateEffect();
            j++;
        }
        skillsUI.UpdateWeaponReloadVisualCooldown(weaponCooldownsProportion, equippedWeapon.weaponIndex);

        if (equippedWeapon.logic != null)
        {
            equippedWeapon.logic.UpdateEquippedEffect();
        }

        // Update effect of passive skills
        foreach (var s in skills)
        {
            if (s is PassiveSkill)
            {
                s.UpdateEffect();
            }
        }
    }

    public void ReloadWeaponIfNeeded()
    {
        if (equippedWeapon.reloadTimeLeft == 0)
        {
            equippedWeapon.reloadTimeLeft = equippedWeapon.logic.reloadTime *
                Mathf.Lerp(1, 0.5f, (float)equippedWeapon.ammoLeft / equippedWeapon.logic.ammoMagazine); // more bullets = faster reload
        }
    }

    #region UI block
    private void InitializeUI()
    {
        ApplyWeaponSprites();
        ApplySkillSprites();
    }

    private void ApplyWeaponSprites()
    {
        var weaponIcons = new Sprite[SkillsUI.weaponsCount];
        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            weaponIcons[i] = equippedWeapons[i].logic.pickupSprite;
        }
        skillsUI.SetWeaponSprites(weaponIcons, equippedWeapon.weaponIndex);
    }

    private void ApplySkillSprites()
    {
        var skillIcons = new Sprite[SkillsUI.skillCount];
        for (int i = 0; i < activeSkills.Count; i++)
        {
            if (activeSkills[i] != null)
            {
                skillIcons[i] = activeSkills[i].skill.pickupSprite;
            }
        }
        skillsUI.SetSkillSprites(skillIcons);
    }
    #endregion

    public List<SkillBase> skills = new List<SkillBase>();

    private List<EquippedActiveSkill> activeSkills = new List<EquippedActiveSkill>();

    private List<EquippedWeapon> equippedWeapons = new List<EquippedWeapon>();
    private KeyCode rotateWeaponLeft = KeyCode.Q;
    private KeyCode rotateWeaponRight = KeyCode.E;
    private CharacterShooting attackManager;
    private SkillsUI skillsUI;
}
