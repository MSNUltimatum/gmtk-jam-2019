using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch1BossLevelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bossSpawnEffect = null;
    [SerializeField]
    private GameObject BossPrefab = null;
    private GameObject BossInstance;

    new private Transform camera;
    enum Phase
    {
        INACTIVE,
        INTRO,
        PRE_PHASE1,
        PHASE1,
    }

    // Start is called before the first frame update
    private Phase CurrentPhase = Phase.INTRO;

    private void Start()
    {
        CurrentEnemy.SetCurrentEnemyName("???");
        camera = Camera.main.transform;
    }

    private void Update()
    {
        switch (CurrentPhase)
        {
            case Phase.INACTIVE:
                break;
            case Phase.INTRO:
                UpdateIntro();
                break;
            case Phase.PRE_PHASE1:
                UpdatePrePhase1();
                break;
            default:
                break;
        }
    }

    public void StartFight()
    {
        CurrentPhase = Phase.PRE_PHASE1;
        Instantiate(bossSpawnEffect, new Vector3(0, 16.5f, 0), Quaternion.identity);
    }

    private void UpdateIntro()
    {

    }

    private float phasePre1TimeToBossSpawn = 2f;

    private void UpdatePrePhase1()
    {
        phasePre1TimeToBossSpawn -= Time.deltaTime;
        if (phasePre1TimeToBossSpawn <= 0)
        {
            CurrentPhase = Phase.PHASE1;
            Instantiate(BossPrefab, new Vector3(0, 16.5f, 0), Quaternion.identity);
        }
    }
}
