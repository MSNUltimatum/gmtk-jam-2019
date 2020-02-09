using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TriplePistol", menuName = "ScriptableObject/Weapon/TriplePistol", order = 1)]
public class TriplePistol : Pistol
{
    private float arcAngle = 45;
    private int shotNumber = 3;

    // этот скрипт поломан, не смотреть
    protected TriplePistol() : base()
    {
        description = "Your second gun";
    }

    public override void Attack(CharacterShooting attackManager, Vector3 mousePos, Vector3 screenPoint)
    {
        for (int i = 0; i < shotNumber; i++)
        {
            var bullet = GameObject.Instantiate(currentBulletPrefab, Player.transform.position, new Quaternion());
            BulletInit(bullet);
            var offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            angle += Mathf.Lerp(-arcAngle / 2, arcAngle / 2, i / (shotNumber - 1.0f));
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
            bullet.transform.Translate(Vector2.right * 0.5f);
        }

        shootingEvents?.Invoke();
    }
}
