using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkill : MonoBehaviour
{
    public string Name;
    public string Description;
    public WeaponData data;
    public float ReloadTime;
    public virtual void Shoot(Vector3 mousePos, Vector3 screenPoint)
    {
    }
}
