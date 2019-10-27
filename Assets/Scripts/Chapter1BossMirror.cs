using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1BossMirror : MirrorTriggerScript
{
    [SerializeField]
    private Sprite MirrorSpriteActive;
    [SerializeField]
    private Sprite MirrorSpriteInactive;

    private bool isActive = false;

    private float timer = 0;

    protected override void ActivateMirrorEffect(GameObject objectNearMirror)
    {
        base.ActivateMirrorEffect(objectNearMirror);
        isActive = true;
        Mirror.GetComponent<SpriteRenderer>().sprite = MirrorSpriteActive;
    }

    private void Update()
    {
        timer = isActive ? timer + Time.deltaTime : 0;
    }

    protected override void DeactivateMirrorEffect()
    {
        base.DeactivateMirrorEffect();
        isActive = false;
        Mirror.GetComponent<SpriteRenderer>().sprite = MirrorSpriteInactive;
    }
}
