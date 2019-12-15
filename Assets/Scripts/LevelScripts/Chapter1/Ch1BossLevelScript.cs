using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Ch1BossLevelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bossSpawnEffect = null;
    [SerializeField]
    private GameObject BossPrefab = null;
    private GameObject BossInstance;
    private GameObject Player;

    new private Transform camera;
    enum Phase
    {
        INACTIVE,
        INTRO,
        PRE_PHASE1,
        PHASE1,
        PHASE2,
        PHASE4,
        DEAD
    }

    // Start is called before the first frame update
    private Phase CurrentPhase = Phase.INTRO;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        AudioManager.Pause("Chapter1BossMusic", GetComponent<AudioSource>());
        CurrentEnemy.SetCurrentEnemyName("???");
        camera = Camera.main.transform;
    }

    private float timeElapsed;

    private void Update()
    {
        if (CharacterLife.isDeath) return;
        timeElapsed += Time.deltaTime;
        switch (CurrentPhase)
        {
            case Phase.INACTIVE:
                timeElapsed = 0;
                break;
            case Phase.INTRO:
                UpdateIntro();
                break;
            case Phase.PRE_PHASE1:
                UpdatePrePhase1();
                break;
            case Phase.PHASE1:
                UpdatePhase1();
                break;
            case Phase.PHASE2:
                UpdatePhase2();
                break;
            case Phase.PHASE4:
                UpdatePhase4();
                break;
            case Phase.DEAD:
                UpdateDead();
                break;
            default:
                break;
        }
    }

    public void StartFight()
    {
        Pause.AllowPause = false;
        CurrentPhase = Phase.PRE_PHASE1;
        Instantiate(bossSpawnEffect, new Vector3(0, 16.5f, 0), Quaternion.identity);
    }

    private void UpdateIntro()
    {
        timeElapsed = 0;
    }

    enum Phase1Attack
    {
        IDLE,
        MoveExplode
    }

    [SerializeField]
    private GameObject BossBulletMiddle = null;
    [SerializeField]
    private GameObject HomingBullet = null;

    private void BossCircleBulletBurst(int projectilesCount, GameObject bulletPrefab)
    {
        var bossRotationZ = BossInstance.transform.rotation.eulerAngles.z;
        bossRotationZ += Random.Range(-20, 20);
        var bossPosition = BossInstance.transform.position;
        for (float i = bossRotationZ; i < bossRotationZ + 360; i += 360 / projectilesCount)
        {
            Instantiate(bulletPrefab, bossPosition, Quaternion.Euler(new Vector3(0, 0, i)));
        }
    }

    float angleSign = 1;

    private void ShootHomingBullet(float plusAngle, GameObject bulletPrefab)
    {
        var bossPosition = BossInstance.transform.position;
        var bulletRotation = BossInstance.transform.rotation.eulerAngles.z + 90;
        bulletRotation += plusAngle * angleSign;
        //var rotateAngle = (bulletRotation < 180) ? bulletRotation + 360 : bulletRotation;
        //rotateAngle += plusAngle * angleSign;
        Instantiate(bulletPrefab, bossPosition, 
            Quaternion.Euler(new Vector3(0, 0, bulletRotation)));
        angleSign *= -1;
    }
    
    [SerializeField]
    private GameObject Phase1PlayerNameLabel = null;
    private Vector3 Phase1StartingPosition;
    private Vector3 Phase1PositionToMoveTo;
    private Vector3 Phase1MoveStartPosition;
    private float Phase1TimeElapsed = 0;
    private float Phase1IdleTime = 1;
    private Phase1Attack Phase1CurrentAttack = Phase1Attack.IDLE;
    private float Phase1TimeToExplosion = 0.0f;
    private int Phase1ExplosionsCount;
    private float Phase1TimeToHomingShooting = 5.1f;
    private float HomingShotCount = 0;
    private float Phase1MoveTimeElapsed = 0;

    [SerializeField]
    private Tilemap BossRoomEntrance = null;

    private void StartPhase1()
    {
        /////////////////////////////////////
        /////////////////////////////////////
        /////////////////////////////////////
        /////////////////////////////////////
        /////////////////////////////////////
        /////////////////////////////////////
        //timeElapsed = 20f;
        
        CurrentPhase = Phase.PHASE1;
        BossInstance = Instantiate(BossPrefab, new Vector3(0, 16.5f, 0), Quaternion.identity);
        AudioManager.Play("Chapter1BossMusic", GetComponent<AudioSource>());
        Phase1PlayerNameLabel.SetActive(true);
        
        var bossPosition = BossInstance.transform.position;
        Phase1StartingPosition = bossPosition;
        Phase1MoveStartPosition = bossPosition;
        Phase1PositionToMoveTo = new Vector3(
            bossPosition.x + Mathf.Sign(Random.Range(-1, 1)) * 10, bossPosition.y, bossPosition.z);

        Phase1TimeToHomingShooting = 5.1f - Phase1IdleTime;
        CurrentEnemy.SetCurrentEnemyName("Survive!");
    }

    [SerializeField]
    private int BigBurstBulletAmount = 12;
    [SerializeField]
    private int MediumBurstBulletAmount = 8;

    private void UpdatePhase1()
    {
        Phase1TimeElapsed += Time.deltaTime;
        switch (Phase1CurrentAttack)
        {
            case Phase1Attack.IDLE:
                if (Phase1TimeElapsed > Phase1IdleTime)
                {
                    Phase1CurrentAttack = Phase1Attack.MoveExplode;
                }
                break;
            case Phase1Attack.MoveExplode:
                // Move part
                Phase1MoveTimeElapsed += Time.deltaTime;
                BossInstance.transform.position = Vector3.Lerp(Phase1MoveStartPosition, Phase1PositionToMoveTo, Phase1MoveTimeElapsed);
                if (Phase1MoveTimeElapsed > 5)
                {
                    if (HomingShotCount < 7)
                    {
                        Phase1MoveTimeElapsed = 0;
                        Phase1MoveStartPosition = BossInstance.transform.position;
                        Phase1PositionToMoveTo = new Vector3(
                            -Mathf.Sign(Phase1PositionToMoveTo.x) * 10 + Phase1StartingPosition.x,
                            Phase1StartingPosition.y,
                            Phase1StartingPosition.z);
                    }
                    if (timeElapsed > 16)
                    {
                        Phase1MoveTimeElapsed = 0;
                        Phase1MoveStartPosition = BossInstance.transform.position;
                        Phase1PositionToMoveTo = new Vector3(
                            Phase1StartingPosition.x,
                            Phase1StartingPosition.y - 2,
                            Phase1StartingPosition.z);
                    }
                }

                // Explosions part
                Phase1TimeToExplosion -= Time.deltaTime;
                if (Phase1TimeToExplosion <= 0)
                {
                    Phase1TimeToExplosion = 0.65f;
                    if (timeElapsed < 5)
                    {
                        BossCircleBulletBurst(BigBurstBulletAmount, BossBulletMiddle);
                    }
                    else
                    {
                        BossCircleBulletBurst(MediumBurstBulletAmount, BossBulletMiddle);
                    }
                    
                }

                // Homing shot part
                Phase1TimeToHomingShooting -= Time.deltaTime;
                if (Phase1TimeToHomingShooting <= 0)
                {
                    HomingShotCount++;
                    ShootHomingBullet(60 - HomingShotCount * 10, HomingBullet);
                    Phase1TimeToHomingShooting = 0.5f;
                    if (HomingShotCount == 6)
                    {
                        Phase1TimeToHomingShooting = 4f;
                    }
                    if (HomingShotCount == 12)
                    {
                        HomingShotCount = 3;
                        Phase1TimeToHomingShooting = 1f;
                    }
                }

                // Switch phase part
                if (timeElapsed > 18)
                {
                    StartPhase2();
                }
                break;
            default:
                break;
        }
    }

    private float phasePre1TimeToBossSpawn = 2f;

    private void UpdatePrePhase1()
    {
        timeElapsed = 0;
        phasePre1TimeToBossSpawn -= Time.deltaTime;
        BossRoomEntrance.gameObject.SetActive(true);
        BossRoomEntrance.color = new Color(
            BossRoomEntrance.color.r, BossRoomEntrance.color.g, BossRoomEntrance.color.b,
            Mathf.Lerp(1, 0, phasePre1TimeToBossSpawn / 2));
        if (phasePre1TimeToBossSpawn <= 0)
        {
            StartPhase1();
        }
    }

    enum Phase2Attack
    {
        GlassStart,
        MonsterAttack,
        GlassFade
    }

    [SerializeField]
    private SpriteRenderer GlassSprite = null;
    private Phase2Attack Phase2CurrentAttack;
    private float GlassStartDuration = 1.5f;
    private float GlassStartTimePassed;
    private float GlassFadeOutDuration = 1f;
    private float GlassFadeOutPassed;
    [SerializeField]
    private SpawnZoneScript leftSpawnZone = null;
    [SerializeField]
    private SpawnZoneScript rightSpawnZone = null;

    private void StartPhase2()
    {
        CurrentPhase = Phase.PHASE2;
        BossInstance.SetActive(false);
        print(BossInstance.name);
        Phase2CurrentAttack = Phase2Attack.GlassStart;
        GlassStartTimePassed = 0;
        monsterSpawner = GetComponent<ArenaEnemySpawner>();
    }

    [SerializeField]
    private GameObject[] Phase2Monsters = null;
    private float Phase2TimeToMonsterSpawn = 0.0f;
    private int Phase2SpawnIndex = 0;
    private ArenaEnemySpawner monsterSpawner;
    [SerializeField]
    private float Phase2TimeToEachMonsterSpawn = 1f;

    private void UpdatePhase2()
    {
        switch (Phase2CurrentAttack)
        {
            case Phase2Attack.GlassStart:
                GlassStartTimePassed += Time.deltaTime;
                GlassSprite.color = new Color(
                    GlassSprite.color.r, GlassSprite.color.g, GlassSprite.color.b,
                    Mathf.Lerp(0, 0.5f, GlassStartTimePassed / GlassStartDuration));
                if (GlassStartTimePassed >= GlassStartDuration)
                {
                    Phase2CurrentAttack = Phase2Attack.MonsterAttack;
                }
                break;
            case Phase2Attack.MonsterAttack:
                Phase2TimeToMonsterSpawn -= Time.deltaTime;
                if (Phase2TimeToMonsterSpawn <= 0)
                {
                    // Choosing spawn zone
                    SpawnZoneScript spawnZone = null;
                    var dice = Random.Range(-1.0f, 1.0f); // -1 or 0
                    spawnZone = dice < 0 ? leftSpawnZone : rightSpawnZone;
                    monsterSpawner.SpawnZone = spawnZone;

                    Phase2TimeToMonsterSpawn = Phase2TimeToEachMonsterSpawn;
                    if (Phase2SpawnIndex < Phase2Monsters.Length)
                    {
                        var monsterToSpawn = Phase2Monsters[Phase2SpawnIndex];
                        var enemy = monsterSpawner.SpawnCertainMonsterWithoutName(monsterToSpawn);
                        enemy.GetComponent<MonsterLife>().FadeIn(0.3f);
                    }
                    else
                    {
                        Phase2CurrentAttack = Phase2Attack.GlassFade;
                        GlassFadeOutPassed = 0;
                    }
                    Phase2SpawnIndex++;
                }
                break;
            case Phase2Attack.GlassFade:
                GlassFadeOutPassed += Time.deltaTime;
                if (GlassFadeOutPassed < GlassFadeOutDuration)
                {
                    GlassSprite.color = new Color(
                        GlassSprite.color.r, GlassSprite.color.g, GlassSprite.color.b,
                        Mathf.Lerp(0.5f, 1, GlassFadeOutPassed / GlassFadeOutDuration));
                }
                
                if (GlassFadeOutPassed / GlassFadeOutDuration > 1)
                {
                    StartPhase4();
                }
                break;
            default:
                break;
        }
    }

    enum Phase4Attack
    {
        Idle,
        Spawn,
        Attack
    }

    private Phase4Attack phase4Attack;
    private float idleWaitTime = 1.24f;
    private Vector3 phase4SpawnPosition;
    private MonsterLife bossScript;
    [SerializeField]
    private float phase4FadeIn = 1.5f;
    private float phase4FadeInLeft;
    [SerializeField]
    private GameObject MirrorCrack;
    [SerializeField]
    private int BurstBulletAmount = 36;
    [SerializeField]
    private Vector2 BulletSpeedRange = new Vector2(5, 15);
    [SerializeField]
    private float AfterBossFadeInExtraTime = 0.3f;
    [SerializeField]
    private float Phase4AfterAttackIdleTime = 2.75f;

    private void StartPhase4()
    {
        MirrorCrack.SetActive(true);
        CurrentPhase = Phase.PHASE4;
        phase4Attack = Phase4Attack.Idle;
        phase4SpawnPosition = Phase4CalculateSpawnPosition();
        
        Player.GetComponentInChildren<TMPro.TextMeshPro>().text = "Hero";
        var bossName = BossInstance.GetComponentInChildren<TMPro.TextMeshPro>();
        bossName.text = "Shadow";
        var labelPosition = bossName.GetComponent<StopRotation>();
        labelPosition.baseEulerRotation = new Vector3(0, 180, 0);
        labelPosition.offset = new Vector3(
            -labelPosition.offset.x, labelPosition.offset.y, labelPosition.offset.z);
        
        BossInstance.SetActive(true);
        bossScript = BossInstance.GetComponent<MonsterLife>();
        bossScript.FadeIn(phase4FadeIn);
        bossScript.MakeBoy();

        GlassFadeOutDuration = GlassFadeOutDuration / 3;
        GlassFadeOutPassed = 0;
    }

    private void UpdatePhase4()
    {
        GlassFadeOutPassed += Time.deltaTime;
        GlassSprite.color = new Color(
            GlassSprite.color.r, GlassSprite.color.g, GlassSprite.color.b,
            Mathf.Lerp(1, 0, GlassFadeOutPassed / GlassFadeOutDuration));

        if (BossInstance == null)
        {
            StartDead();
            CurrentEnemy.SetCurrentEnemyName(" ");
            return;
        }
        if (ArenaEnemySpawner.boysList.Count == 0)
        {
            CurrentEnemy.SetCurrentEnemyName("Shadow");
        }
        switch (phase4Attack)
        {
            case Phase4Attack.Idle:
                idleWaitTime -= Time.deltaTime;
                if (idleWaitTime <= 0)
                {
                    phase4Attack = Phase4Attack.Spawn;
                    phase4FadeInLeft = phase4FadeIn;
                    Phase4Spawn();
                }
                break;
            case Phase4Attack.Spawn:
                bossScript.FadeIn(phase4FadeIn);
                phase4FadeInLeft -= Time.deltaTime;
                if (phase4FadeInLeft + AfterBossFadeInExtraTime <= 0)
                {
                    phase4FadeInLeft = phase4FadeIn;
                    phase4Attack = Phase4Attack.Attack;
                }
                break;
            case Phase4Attack.Attack:
                idleWaitTime = Phase4AfterAttackIdleTime;
                Random360Burst(BurstBulletAmount);
                phase4Attack = Phase4Attack.Idle;
                break;
            default:
                break;
        }
    }

    private Vector3 Phase4CalculateSpawnPosition()
    {
        return new Vector3(
            Random.Range(-10, 10),
            Random.Range(7, 12),
            0);
    }

    private void Phase4Spawn()
    {
        phase4SpawnPosition = Phase4CalculateSpawnPosition();
        BossInstance.transform.position = phase4SpawnPosition;
        Instantiate(bossSpawnEffect, phase4SpawnPosition, Quaternion.identity);
    }

    private void Random360Burst(int bullets)
    {
        for (int i = 0; i < bullets; i++)
        {
            var bullet = Instantiate(BossBulletMiddle, BossInstance.transform.position,
                Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            bullet.GetComponent<EnemyBulletLife>().BulletSpeed = Random.Range(BulletSpeedRange.x, BulletSpeedRange.y);
        }
    }

    private void StartDead()
    {
        GlassStartDuration = 3;
        GlassStartTimePassed = 0;
        CurrentPhase = Phase.DEAD;
    }

    [SerializeField]
    private string NextSceneName = "FinalCredits";

    private void UpdateDead()
    {
        GlassStartTimePassed += Time.deltaTime;
        GlassSprite.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, GlassStartTimePassed / GlassStartDuration));
        if (GlassStartTimePassed / GlassStartDuration > 1)
        {
            SceneManager.LoadScene(NextSceneName);
        }
    }
}
