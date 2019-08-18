using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovableEnemy : EnemyMovement
{
    Vector3 direct;
    private float CoolDownBefore;
    [SerializeField]
    private float CoolDown = 1f;
    [SerializeField]
    private Vector2 RangeOfMotion = new Vector2(0, 5);

    [SerializeField]
    private float RandomImpact = 5f;

    protected override void Start()
    {
        base.Start();
        CoolDownBefore = CoolDown;
        direct = Player.transform.position;
    }

    protected override void Update()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        NewDirect();
        MoveAndRotate();
    }

    private void NewDirect()
    {
        if (CoolDownBefore == 0) {

            if (!soundLock)
            {
                var audio = FindObjectOfType<AudioManager>();
                audio.Play("Cockroach");
                soundLock = true;
            }
            var vect = Player.transform.position + direct / 2;
            vect.Normalize();
            var distanceBasedValue = RandomImpact * Vector2.Distance(Player.transform.position, transform.position); 
            vect.x += Random.Range(-distanceBasedValue, distanceBasedValue);
            vect.y += Random.Range(-distanceBasedValue, distanceBasedValue);
            direct = vect + Player.transform.position;

            CoolDownBefore = Random.Range(CoolDown / 3, CoolDown);

        }
    }
    private bool soundLock = false;
}
