using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch1BossLevelScript : MonoBehaviour
{
    enum Phase
    {
        INTRO,

    }

    // Start is called before the first frame update
    private Phase CurrentPhase = Phase.INTRO;

    private void Start()
    {
        CurrentEnemy.SetCurrentEnemyName("???");
    }

    private void Update()
    {
        switch (CurrentPhase)
        {
            case Phase.INTRO:
                break;
            default:
                break;
        }
    }
}
