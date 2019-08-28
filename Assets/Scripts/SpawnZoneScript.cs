using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneScript : MonoBehaviour
{
    public Vector2 SpawnPosition()
    {
        Vector2 vector = new Vector2(Random.Range(-gameObject.transform.localScale.x/2, 
            gameObject.transform.localScale.x/2) + gameObject.transform.position.x,
            Random.Range(-gameObject.transform.localScale.y/2, 
            gameObject.transform.localScale.y/2) + gameObject.transform.position.y);
        //Debug.Log(vector);
        return vector;
    }
}
