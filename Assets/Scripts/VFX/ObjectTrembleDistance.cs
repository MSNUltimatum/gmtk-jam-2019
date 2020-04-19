using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrembleDistance : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectToCheckDistance = null;
    [SerializeField] private Vector2 trembleDynamicRange = new Vector2(5, 10);
    [SerializeField] private float trembleAmplitude = 0.25f;
    [SerializeField] private float tremblesInSecond = 8;

    private void Awake()
    {
        startingPosition = transform.position;
        movingFrom = transform.position;
        movingTo = GetNewTremblePosition();
    }

    void Update()
    {
        if (Time.time > nextTrembleTime)
        {
            movingFrom = transform.position;
            movingTo = startingPosition + GetNewTremblePosition();
        }
        transform.position = Vector3.Lerp(movingTo, movingFrom, (nextTrembleTime - Time.time) * tremblesInSecond);
    }

    private Vector3 GetNewTremblePosition()
    {
        nextTrembleTime += 1 / tremblesInSecond;
        var trembleAmp = trembleAmplitude * Mathf.InverseLerp(
                trembleDynamicRange.y, trembleDynamicRange.x,
                Vector3.Distance(gameObjectToCheckDistance.transform.position, transform.position));
        return new Vector3(Random.Range(-trembleAmp, trembleAmp), Random.Range(-trembleAmp, trembleAmp), 0);
    }

    private Vector3 startingPosition;
    private Vector3 movingFrom;
    private Vector3 movingTo;
    private float nextTrembleTime = 0;
}
