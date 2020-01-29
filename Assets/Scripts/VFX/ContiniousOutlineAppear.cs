using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContiniousOutlineAppear : MonoBehaviour
{
    public bool activated { get; private set; } = false;
    private float accumulator = 0;
    private void Start()
    {
        continiousOutlineMaterial = GetComponent<SpriteRenderer>().material;
        continiousOutlineMaterial = new Material(continiousOutlineMaterial);
        if (continiousOutlineMaterial.shader.name != "Unlit/ContiniousWithOutline")
        {
            Debug.LogError("Material shader: " + continiousOutlineMaterial.shader.name + 
                " is not ContiniousOutline shader on object: " + gameObject.name);
            activated = false;
        }
        
    }
    
    private void Update()
    {
        if (activated)
        {
            accumulator = Accumulate(accumulator);
            continiousOutlineMaterial.SetFloat("_OutlineAppearParameter", accumulator);
        }        
    }

    protected float Accumulate(float accumulator)
    {
        return accumulator + Mathf.Abs(Mathf.Sin(Time.time * 4)) * Time.deltaTime / 13;
    }

    public void Activate()
    {
        activated = true;
    }
    
    private Material continiousOutlineMaterial;
}
