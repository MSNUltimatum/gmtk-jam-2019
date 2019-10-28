using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch1BossLevelScript : MonoBehaviour
{
    private Transform camera;
    enum Phase
    {
        INACTIVE,
        INTRO,
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
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        switch (CurrentPhase)
        {
            case Phase.INTRO:
                CameraShakeUpdate();
                break;
            default:
                break;
        }
    }

    private float shakeDuration = 0;
    private float shakeAmount = 0.7f;

    private void CameraShakeUpdate()
    {
        
    }
}
