using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLife : MonoBehaviour
{
    bool THE_BOY = false;

    public void Damage()
    {
        if (THE_BOY)
        {
            GameObject.Find("Game Manager").GetComponent<ArenaEnemySpawner>().ChangeTheBoy(gameObject);
            // TODO: add EXPLOSION, MOTHERF!$&*ER
            Destroy(gameObject);
        }
        else
        {
            // TODO: make visual and sound effects of absorb
        }
    }

    public void MakeBoy()
    {
        THE_BOY = true;
    }
}
