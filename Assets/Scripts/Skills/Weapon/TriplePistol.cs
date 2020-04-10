using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TriplePistol", menuName = "ScriptableObject/Weapon/TriplePistol", order = 1)]
public class TriplePistol : Pistol
{
    private float arcAngle = 45;
    private int shotNumber = 3;
    
    protected TriplePistol() : base()
    {
        description = "Your second gun";
    }

    public override void Attack(CharacterShooting attackManager, Vector3 mousePos)
    {
        for (int i = 0; i < shotNumber; i++)
        {
            var bullet = GameObject.Instantiate(currentBulletPrefab, attackManager.weaponTip.position, 
                Quaternion.Euler(0, 0, attackManager.weaponTip.rotation.eulerAngles.z + 90 + Mathf.Lerp(-arcAngle / 2, arcAngle / 2, i / (shotNumber - 1.0f))));
            BulletInit(bullet);
        }

        shootingEvents?.Invoke();
    }
}
