using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Chapter1BossMirror : MirrorTriggerScript
{
    [SerializeField]
    private SpriteRenderer BossSprite = null;
    [SerializeField]
    private ParticleSystem mirrorActivateParticles = null;

    private bool isActive = false;

    private float timer = 0;
    [SerializeField]
    private float timeToActivate = 7;
    private bool finallyActivated = false;

    private Ch1BossLevelScript bossScript;

    protected override void ActivateMirrorEffect(GameObject objectNearMirror)
    {
        base.ActivateMirrorEffect(objectNearMirror);
        isActive = true;
        mirrorActivateParticles.Play();
    }

    private void Start()
    {
        lightSource = GetComponentInParent<Light2D>();
        startingLightIntensity = lightSource.intensity;
        bossScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<Ch1BossLevelScript>();
    }

    private void Update()
    {
        if (finallyActivated)
        {
            if (timer != 0)
            {
                timer = 0;
                DeactivateMirrorEffect();
                DestroyMirror();
                bossScript.StartFight();
            }
            return;
        }

        timer = isActive ? timer + Time.deltaTime : 0;

        if (timer == 0) return;
        lightSource.intensity = Mathf.Lerp(startingLightIntensity, 2, timer / (timeToActivate * 0.8f));
        BossSprite.color = new Color(
            BossSprite.color.r, BossSprite.color.g, BossSprite.color.b,
            Mathf.Lerp(0, 1, timer / (timeToActivate * 0.8f)));
        if (timer / timeToActivate > 1)
        {
            finallyActivated = true;
        }
    }

    protected override void DeactivateMirrorEffect()
    {
        base.DeactivateMirrorEffect();
        isActive = false;
        lightSource.intensity = startingLightIntensity;
        BossSprite.color = new Color(1, 1, 1, 0);
        mirrorActivateParticles.Stop();
    }

    private void DestroyMirror()
    {

    }

    private Light2D lightSource;
    private float startingLightIntensity;
}
