using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyMovement : MonoBehaviour
{
    [SerializeField]
    private float flightZoneRadius = 3f;

    private void Start()
    {
        startingPoint = transform.position;
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
    
    private void Update()
    {
        ActiveMovement();
    }

    private void ActiveMovement()
    {
        transform.Translate(transform.up * Time.deltaTime, Space.World);
        
        if (timeToRecalculate < 0)
        {
            rotateValue = rotateValue == 0 ? Random.Range(-90, 90) : Random.Range(-90, 0) * Mathf.Sign(rotateValue);
            timeToRecalculate = timeToEachRecalc;
        }

        if (Vector3.Distance(transform.position, startingPoint) < flightZoneRadius)
        {
            timeToRecalculate -= Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, rotateValue * Time.deltaTime));
        }
        else
        {
            rotateValue = 0;
            var angle = Vector3.SignedAngle(transform.up, startingPoint - transform.position, Vector3.one) * Time.deltaTime;
            angle += transform.eulerAngles.z;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    private float rotateValue = 0;
    private float timeToRecalculate = 1f;
    private float timeToEachRecalc = 1f;
    private Vector3 startingPoint;
}
