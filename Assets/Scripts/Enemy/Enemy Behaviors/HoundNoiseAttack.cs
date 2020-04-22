using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HoundNoiseAttack : Attack
{
    [SerializeField] private Vector2 noiseRangeMinMax = new Vector2(2, 10);

    protected override void Awake()
    {
        base.Awake();
        postProcessVolume = GetComponentInChildren<PostProcessVolume>();
    }

    protected override void DoAttack()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
        var noiseValue = Mathf.InverseLerp(noiseRangeMinMax.y, noiseRangeMinMax.x, distanceToPlayer);
        postProcessVolume.weight = noiseValue;
    }

    private float noiseFactor;
    private PostProcessVolume postProcessVolume;
}
