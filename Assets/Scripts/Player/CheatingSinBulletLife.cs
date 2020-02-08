using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatingSinBulletLife : BulletLife
{
    public float Speed 
    { 
        set 
        { 
            for(int i = 0;i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<BulletLife>().speed = value;
            }
        }
    }

    public float timeToDestruction
    {
        set
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).GetComponent<BulletLife>().timeToDestruction = value;
            }
        }
    }
}
