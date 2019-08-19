using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotation : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset = new Vector3(-0.65f, 1.22f);
    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + offset;
    }
}
