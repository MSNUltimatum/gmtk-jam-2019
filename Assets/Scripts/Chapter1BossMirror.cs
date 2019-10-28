using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1BossMirror : MirrorTriggerScript
{
    [SerializeField]
    private Sprite MirrorSpriteActive = null;
    [SerializeField]
    private Sprite MirrorSpriteInactive = null;
    [SerializeField]
    private ParticleSystem mirrorActivateParticles = null;

    private bool isActive = false;

    private float timer = 0;
    private float timeToActivate = 7;
    private bool finallyActivated = false;

    protected override void ActivateMirrorEffect(GameObject objectNearMirror)
    {
        base.ActivateMirrorEffect(objectNearMirror);
        isActive = true;
        Mirror.GetComponent<SpriteRenderer>().sprite = MirrorSpriteActive;
        mirrorActivateParticles.Play();
    }

    private void Update()
    {
        if (finallyActivated && timer != 0)
        {
            timer = 0;
            DeactivateMirrorEffect();
            return;
        }

        timer = isActive ? timer + Time.deltaTime : 0;

        if (timer == 0) return;
    }

    protected override void DeactivateMirrorEffect()
    {
        base.DeactivateMirrorEffect();
        isActive = false;
        Mirror.GetComponent<SpriteRenderer>().sprite = MirrorSpriteInactive;
        mirrorActivateParticles.Stop();
    }

    private void DestroyMirror()
    {

    }
}
