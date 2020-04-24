using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderLoopMovement : MonoBehaviour
{
    private Dictionary<Direction.Side, float> borders;
    [SerializeField] private float stepFromBordersUp = 10f;
    [SerializeField] private float stepFromBordersHorizontal = 10f;
    [SerializeField] private float verticalLoopSize = 10f;
    private float phase = 0;
    [SerializeField] private float period = 5f;

    private enum Status { move, brake, shoot, accelerate };
    private Status status = Status.move;

    [SerializeField] private float moveTime = 8f;
    [SerializeField] private float brakeTime = 0.5f;
    [SerializeField] private float shootTime = 3f;
    [SerializeField] private float accelTime = 0.5f;

    [SerializeField] private float timerToStatusChange = 0;

    [SerializeField] private EnemyLaser laser;
    private Vector3 stayPosition;


    private void Awake()
    {
        borders = GetComponent<MonsterLife>().monsterManager.room.GetBordersFromTilemap();
        //добавить эксепшн для арена спавнера без комнат
        timerToStatusChange = moveTime;
        if (laser == null) laser = GetComponentInChildren<EnemyLaser>();
        if (laser == null) Debug.Log("HarpyQueen cant find laser");
    }

    private void Update()
    {
        if (!Pause.Paused)
        {
            if (status == Status.move)
            {
                transform.position = GetPositionFromPhase(phase);
                if (period != 0)
                    phase += Time.deltaTime * 2 * Mathf.PI / period;

                timerToStatusChange -= Time.deltaTime;
                if (timerToStatusChange <= 0) {
                    status = Status.brake;
                    timerToStatusChange = brakeTime;
                }
            }
            else if (status == Status.brake)
            {
                transform.position = GetPositionFromPhase(phase);
                if (period != 0)
                    phase += (Time.deltaTime * 2 * Mathf.PI / period) * (timerToStatusChange / brakeTime);
                timerToStatusChange -= Time.deltaTime;
                if (timerToStatusChange <= 0)
                {
                    status = Status.shoot;
                    timerToStatusChange = shootTime;
                    ShootLaser();
                    stayPosition = transform.position;
                }
            }
            else if (status == Status.shoot)
            {
                transform.position = stayPosition;
                Debug.DrawLine(transform.position, new Vector3(transform.position.x, borders[Direction.Side.DOWN], 0));
                timerToStatusChange -= Time.deltaTime;
                if (timerToStatusChange <= 0)
                {
                    StopLaser();
                    status = Status.accelerate;
                    timerToStatusChange = accelTime;
                }
            }
            else if (status == Status.accelerate)
            {
                transform.position = Vector3.Lerp(transform.position, GetPositionFromPhase(phase), Mathf.Pow(((accelTime - timerToStatusChange) / accelTime),2f)); // lerp^2 to prevent teleport on unfreez
                if (period != 0)
                    phase += (Time.deltaTime * 2 * Mathf.PI / period) * ((accelTime - timerToStatusChange) / accelTime);
                timerToStatusChange -= Time.deltaTime;
                if (timerToStatusChange <= 0)
                {
                    status = Status.move;
                    timerToStatusChange = moveTime;
                }
            }
        }
    }

    private Vector3 GetPositionFromPhase(float phase) {
        float topBorder = Mathf.Min(borders[Direction.Side.UP], Camera.main.ViewportToWorldPoint(Vector3.one).y);
        float leftBorder = Mathf.Max(borders[Direction.Side.LEFT], Camera.main.ViewportToWorldPoint(Vector3.zero).x);
        float rightBorder = Mathf.Min(borders[Direction.Side.RIGHT], Camera.main.ViewportToWorldPoint(Vector3.one).x);
        return new Vector3(
            Mathf.Sin(phase) * ((rightBorder - leftBorder) / 2 - (2 * stepFromBordersHorizontal)) + (leftBorder + rightBorder) / 2f,
            Mathf.Sin(phase * 2) * verticalLoopSize + topBorder - verticalLoopSize - stepFromBordersUp,
            0);
    }

    private void ShootLaser() {
        laser.ShootStart(transform.position, new Vector3(transform.position.x, borders[Direction.Side.DOWN], 0));
    }

    private void StopLaser()
    {
        laser.ShootStop();
    }
}
