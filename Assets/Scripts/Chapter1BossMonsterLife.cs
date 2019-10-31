using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1BossMonsterLife : MonsterLife
{
    protected override bool SpecialConditions(GameObject source)
    {
        return source.GetComponent<Chapter1BossInfusedBullet>() != null;
    }

    protected override void PreDestroyEffect()
    {
        base.PreDestroyEffect();
        AudioManager.Pause("Chapter1BossMusic",
            GameObject.FindWithTag("GameController").GetComponent<AudioSource>());
    }

    protected override void HitEffect()
    {
        base.HitEffect();
        var lightController = GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<RoomLighting>();
        lightController.AddToLight(28);
    }
}
