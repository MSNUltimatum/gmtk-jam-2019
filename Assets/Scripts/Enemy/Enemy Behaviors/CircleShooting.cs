using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab = null;
    private float timer = 0f;

    [SerializeField] private float openTime = 1f;
    [SerializeField] private float shootTime = 1f;
    [SerializeField] private float closeTime = 1f;
    [SerializeField] private float moveTime = 1f;
    [SerializeField] private int bulletsNumber = 20;
    private int bulletsWasShootCounter = 0;
    [SerializeField] private float ramdomAngleRange = 10f;

    private enum Status { move, open, shoot, close };
    private Status status = Status.move;

    private GameObject player;
    private AIAgent agent;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<AIAgent>();
        status = Status.move;
        agent.moveSpeedMult = 1;
    }

    private void Update()
    {
        if (!Pause.Paused)
            if (status == Status.shoot)
            {
                timer -= Time.deltaTime;
                while ((shootTime - timer) / shootTime >= (float)bulletsWasShootCounter / (float)bulletsNumber)
                    ShootBullet();
                if (timer <= 0)
                {
                    //ainmation swich to close?
                    status = Status.close;
                    timer = closeTime;
                    bulletsWasShootCounter = 0;
                }
            }
            else if (status == Status.close)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    //ainmation swich to shoot?
                    status = Status.move;
                    timer = moveTime;
                    agent.moveSpeedMult = 1;
                }
            }
            else if (status == Status.move)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    //ainmation swich to open?
                    status = Status.open;
                    timer = openTime;
                    agent.moveSpeedMult = 0;
                }
            }
            else if (status == Status.open)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    //ainmation swich to move?
                    status = Status.shoot;
                    timer = moveTime;
                }
            }
    }

    private void ShootBullet() {
        Vector3 dirrectionToPlayer = player.transform.position - transform.position;
        float rotatingAngle = (360f / bulletsNumber) * bulletsWasShootCounter + Random.Range(-ramdomAngleRange, ramdomAngleRange)+180;        

        GameObject bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());

        var audio = GetComponent<AudioSource>();
        AudioManager.Play("MonsterShot", audio);

        var angle = Mathf.Atan2(dirrectionToPlayer.y, dirrectionToPlayer.x) * Mathf.Rad2Deg + rotatingAngle;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bulletsWasShootCounter++;        
    }
}
