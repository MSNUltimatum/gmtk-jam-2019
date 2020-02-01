using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField]
    private float timer = 0.5f;

    void Start()
    {
        Destroy(gameObject, timer);
    }
}
