using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingMonsters : MonoBehaviour
{
    [SerializeField]
    private float freezingDuration = 3f;
    public void FreezingShoot(GameObject monster)
    {
        monster.GetComponent<MonsterLife>().enabled = false;
        monster.GetComponent<AIAgent>().enabled = false;
        monster.GetComponentInChildren<Animator>().enabled = false;
        freezingMonsters.Add(monster);
        freezingDurations.Add(freezingDuration);
    }

    private void Freeze()
    {
        for (int i = 0; i < freezingDurations.Count; i++)
        {
            if (freezingMonsters[i] != null)
            {
                freezingDurations[i] -= Time.deltaTime;

                if (freezingDurations[i] <= 0)
                {
                    freezingDurations.RemoveAt(i);
                    freezingMonsters[i].GetComponent<MonsterLife>().enabled = true;
                    freezingMonsters[i].GetComponent<AIAgent>().enabled = true;
                    freezingMonsters[i].GetComponentInChildren<Animator>().enabled = true;
                    freezingMonsters.RemoveAt(i);
                }
            }
            else
            {
                freezingMonsters.RemoveAt(i);
                freezingDurations.RemoveAt(i);
            }
        }
    }
    void Update()
    {
        Freeze();
    }
    private List<GameObject> freezingMonsters = new List<GameObject>();
    private List<float> freezingDurations = new List<float>();
}
