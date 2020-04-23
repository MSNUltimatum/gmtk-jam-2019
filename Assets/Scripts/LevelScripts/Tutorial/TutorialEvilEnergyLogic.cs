using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvilEnergyLogic : MonoBehaviour
{
    public Transform player;
    public Transform monster;
    public float requestedRadius = 1.5f;
    public float speedFlyOut = 3f;

    enum Movement
    {
        FlyOut,
        CircleAround,
        ToMonster
    }

    public void SetEvilLogic(float flyOutSpeed, float requestedRadius, Transform player, Transform monster)
    {
        speedFlyOut = flyOutSpeed;
        this.requestedRadius = requestedRadius;
        this.player = player;
        this.monster = monster;
    }

    void Start()
    {
        flyOutTime = maxFlyOutTime;
        transform.parent = player;
    }

    // Update is called once per frame
    void Update()
    {
        switch (movement)
        {
            case Movement.FlyOut:
                flyOutTime -= Time.deltaTime;
                transform.Translate(transform.right * speedFlyOut * Time.deltaTime * (flyOutTime / maxFlyOutTime));
                if (flyOutTime <= 0) movement = Movement.CircleAround;
                break;
            case Movement.CircleAround:
                circleTime -= Time.deltaTime;
                RecalculateRadius();
                if (Vector3.Distance(player.position, transform.position) > 1f)
                {
                    MoveToOrbit();
                }
                Orbit();
                if (circleTime <= 0) movement = Movement.ToMonster;
                break;
            case Movement.ToMonster:
                ToMonster();
                break;
            default:
                break;
        }
    }

    private void RecalculateRadius()
    {
        radius = Vector3.Distance(player.position, transform.position);
    }

    private void MoveToOrbit()
    {
        transform.Translate(Vector3.Normalize(transform.position - player.position) * Time.deltaTime);
    }

    private void Orbit()
    {
        var x = transform.localPosition.x;
        if (orbitLeft)
            x = Mathf.Max(-radius, x - Time.deltaTime);
        else
            x = Mathf.Min(radius, x + Time.deltaTime);
        if (Mathf.Abs(x) == radius) orbitLeft = !orbitLeft;
        var sqry = radius * radius - x * x;
        var y = Mathf.Sqrt(sqry) * (orbitLeft ? 1 : -1);
        transform.localPosition = new Vector3(x, y, 0);
    }

    private void ToMonster()
    {
        var monsterPos = monster.position;
        var thisPos = transform.position;
        transform.Translate(Vector3.Normalize(thisPos - monsterPos) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            //coll.GetComponent<TutorialFirstEnemySpawned>().AddAlpha();
            Destroy(this);
        }
    }
    
    private float radius = 0;
    private bool orbitLeft = true;
    private Movement movement = Movement.FlyOut;
    private float maxFlyOutTime = 0.5f;
    private float flyOutTime = -1f;
    private float circleTime = 2.5f;
}
