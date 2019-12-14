using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMoveUp : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.5f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
