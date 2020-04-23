using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeDistanceToObject : MonoBehaviour
{
    [SerializeField] private Transform gameObjectToCheckDistance = null;
    [SerializeField] private Vector2 alphaDynamicRange = new Vector2(5, 10);

    private void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
    }
    
    void Update()
    {
        var newc = text.color;
        newc.a = Mathf.InverseLerp(alphaDynamicRange.y, alphaDynamicRange.x, Vector3.Distance(gameObjectToCheckDistance.position, transform.position)) * 0.9f;
        text.color = newc;
    }

    private TextMeshPro text;
}
