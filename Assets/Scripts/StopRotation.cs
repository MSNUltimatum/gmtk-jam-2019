using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + new Vector3(-0.65f, 1.22f);
    }
}
