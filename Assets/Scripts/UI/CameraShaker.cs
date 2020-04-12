using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera shakerVirtualCamera = null;
    private CinemachineBasicMultiChannelPerlin shaker;
    private float shakeAccumulator = 0;
    private Coroutine enumerator;

    void Start()
    {
        shaker = shakerVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        shaker.m_AmplitudeGain = shakeAccumulator;
        // I don't understand why but it works, wow :)
        transform.localEulerAngles = Vector3.zero;
    }

    public void ShakeCamera(float shakeIntensity = 0.5f, float shakeTiming = 0.2f)
    {
        StartCoroutine(ShakeMe(shakeIntensity, shakeTiming));
    }

    private IEnumerator ShakeMe(float shakeIntensity, float shakeTiming)
    {
        shakeAccumulator = shakeIntensity;
        yield return new WaitForSeconds(shakeTiming);
        shakeAccumulator -= shakeIntensity;
    }

}
