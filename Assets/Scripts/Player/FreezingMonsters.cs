using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingMonsters : MonoBehaviour
{
    [SerializeField]
    private float freezingDuration = 3f;
    private float freezingDurationTime;

    private void Start()
    {
        freezingDurationTime = freezingDuration;
        GetComponent<MonsterLife>().enabled = false;
        GetComponent<AIAgent>().enabled = false;
        GetComponentInChildren<Animator>().enabled = false;
    }

    private void Update()
    {
        freezingDurationTime -= Time.deltaTime;
        if(freezingDurationTime <= 0)
        {
            GetComponent<MonsterLife>().enabled = true;
            GetComponent<AIAgent>().enabled = true;
            GetComponentInChildren<Animator>().enabled = true;
            Destroy(this);
        }
    }

    public void Reboot()
    {
        freezingDurationTime = freezingDuration;
    }
}
