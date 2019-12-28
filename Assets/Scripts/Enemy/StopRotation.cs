using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotation : MonoBehaviour
{
    [SerializeField]
    public Vector3 offset = new Vector3(0, 0);

    [SerializeField]
    public Vector3 baseEulerRotation = new Vector3(0, 0, 0);

    // Update is called once per frame
    void LateUpdate()
    {
        transform.eulerAngles = baseEulerRotation;
        if (baseEulerRotation == Vector3.zero) return;
        transform.position = transform.parent.position + offset;
    }
}
