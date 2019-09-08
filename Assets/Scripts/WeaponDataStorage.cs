using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataStorage : MonoBehaviour
{
    public static Dictionary<string, WeaponData> weapon;

    private void Start ()
    {
        weapon = new Dictionary<string, WeaponData>();

        PistolData pistol = new PistolData();
        weapon.Add("Pistol", pistol);
    }

    public static WeaponData GetValue(string name)
    {
        try
        {
            var Val = weapon[name];
            return Val;
        }
        catch
        {
            return null;
        }
    }
}
