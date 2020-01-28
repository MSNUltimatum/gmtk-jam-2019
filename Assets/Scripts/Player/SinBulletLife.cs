using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinBulletLife : BulletLife
{
    [SerializeField]
    bool isCos = false;
    public float frequency = 10.0f; // Скорость виляния по синусоиде
    public float magnitude = 1.5f; // Размер синусоиды (радиус, по сути..можно заменить на "R")

    private Vector3 axis;
    private Vector3 pos;

    void Start()
    {
        pos = transform.position;
        axis = transform.up;
    }

    protected override void Move()
    {
        pos += transform.right * Time.fixedDeltaTime * Speed;
        if (!isCos)
            transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
        else
            transform.position = pos + axis * Mathf.Cos(Time.time * frequency) * magnitude;
    }
}
