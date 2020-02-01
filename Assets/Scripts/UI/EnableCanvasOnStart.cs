using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCanvasOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var canvas = GetComponent<Canvas>();
        canvas.enabled = true;
        canvas.worldCamera = Camera.main;
    }
}
