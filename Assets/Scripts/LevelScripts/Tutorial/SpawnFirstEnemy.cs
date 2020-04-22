using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFirstEnemy : MonoBehaviour
{
    [SerializeField] private GameObject monsterToActivate = null;
    [SerializeField] private GameObject negativeEnergyBolt = null;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (animPreparation >= 0)
        {
            animPreparation -= Time.deltaTime;
        }
        if (monsterAppearLeft >= 0)
        {
            monsterAppearLeft -= Time.deltaTime;
            var newc = monsterSprite.color;
            newc.a = Mathf.InverseLerp(monsterAppearDuration, 0, monsterAppearLeft);
            monsterSprite.color = newc;
        } 
    }

    private void SpawnNegative()
    {

    }

    private IEnumerator SpawnNegativeEnergy()
    {
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
    }

    private GameObject player;
    private SpriteRenderer monsterSprite;
    private float monsterAppearLeft = 0;
    private float monsterAppearDuration = 0.5f;
    private float animPreparation = -1f;
}
