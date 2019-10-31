using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MirrorTriggerScript : MonoBehaviour
{
    [SerializeField]
    protected GameObject Mirror;

    protected virtual void ActivateMirrorEffect(GameObject objectNearMirror) { }

    protected virtual void DeactivateMirrorEffect() { }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            ActivateMirrorEffect(coll.gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            DeactivateMirrorEffect();
        }
    }
}
