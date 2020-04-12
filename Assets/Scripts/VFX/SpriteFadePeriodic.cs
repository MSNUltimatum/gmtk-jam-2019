using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFadePeriodic : MonoBehaviour
{
    [SerializeField]
    private float cycleLength = 1f;
    [SerializeField]
    private float fadeOutTime = 0.13f;
    [SerializeField, Range(0, 1)]
    private float fadeOffset = 0;
    [SerializeField, Range(0, 1)]
    private float fadeMedian = 0;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        accumulator = cycleLength * fadeOffset;
    }

    // Update is called once per frame
    void Update()
    {
        accumulator += Time.deltaTime;
        var newColor = sprite.color;
        newColor.a = Mathf.Clamp01(Mathf.Sin(accumulator / cycleLength * 2 * Mathf.PI) + (1 - fadeOutTime * 2 / cycleLength) + fadeMedian);
        sprite.color = newColor;
    }

    private float accumulator = 0;
    private SpriteRenderer sprite;
}
