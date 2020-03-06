using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PP_Animated : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve weightValue = new AnimationCurve();
    private PostProcessVolume explosionPPV;

    private void Start()
    {
        explosionPPV = GetComponent<PostProcessVolume>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        explosionPPV.weight = weightValue.Evaluate(timer);
    }

    private float timer = 0;
}
