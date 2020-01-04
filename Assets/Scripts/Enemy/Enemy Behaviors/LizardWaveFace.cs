using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWaveFace : Align
{
    [SerializeField]
    private float wavePeriod = 2f; 
    [SerializeField]
    private float waveAmp = 40f;
    [SerializeField]
    private float behaviourBlockTime = 0.5f; // without this may get stuck

    public override EnemySteering GetSteering()
    {
        Vector2 direction = target.transform.position - transform.position;
        if (direction.magnitude > 0.0f) 
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.y);
            targetOrientation *= Mathf.Rad2Deg;
            targetOrientation += WaveFluctuation();
            base.targetOrientation = targetOrientation;
        }

        return base.GetSteering();
    }

    //delta orientation from line of sight to wave trajectory
    private float WaveFluctuation() {
        if (Mathf.Max(behaviourBlockTime -= Time.deltaTime, 0) > 0) return 0;

        wavePhase += Time.deltaTime / wavePeriod;
        if (wavePhase > 1) wavePhase -= 1;
        return Mathf.Sin(wavePhase * 2 * Mathf.PI) * waveAmp;
    }
    
    private float wavePhase = 0f;
}
