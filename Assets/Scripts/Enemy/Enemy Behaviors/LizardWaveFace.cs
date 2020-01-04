using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWaveFace : Align
{
    private float wavePhase = 0f; 
    public float wavePeriod = 2f; 
    public float waveAmp = 40f; 

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
        wavePhase += Time.deltaTime / wavePeriod;
        if (wavePhase > 1) wavePhase -= 1;
        return Mathf.Sin(wavePhase * 2 * Mathf.PI) * waveAmp;
    }

}
