using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingMonsters : MonoBehaviour
{
    [SerializeField]
    private float freezingDuration = 3f;
    private float freezingDurationTime;
    public bool start = false;
    public void MyStart(float time)
    {
        freezingDuration = time;
        freezingDurationTime = freezingDuration;
        Active_Deactivate();
        start = true;
    }

    private void Update()
    {
        if (start)
        {
            freezingDurationTime -= Time.deltaTime;
            if (freezingDurationTime <= 0)
            {
                Active_Deactivate();
                Destroy(this);
            }
        }
    }

    private void Active_Deactivate()
    {
        GetComponent<MonsterLife>().enabled = !GetComponent<MonsterLife>().enabled;
        GetComponent<AIAgent>().enabled = !GetComponent<AIAgent>().enabled;
        foreach (Transform child in transform)
        {
            var tmp = child.GetComponent<Animator>();
            if (tmp)
                tmp.enabled = !tmp.enabled;
        }
    }

    public void Reboot()
    {
        freezingDurationTime = freezingDuration;
    }
}
