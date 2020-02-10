using UnityEngine;
using System.Linq;

public abstract class Align : EnemyBehavior
{
    public float targetRadius;
    public float slowRadius = 0.1f;
    public float timeToTarget = 0.1f;
    public bool rotateAtStart = true;

    protected float maxBypassRaycastDistance = 3f;
    [SerializeField, Header("If bypass is needed choose 50. It is good")]
    protected float bypassAngleAccumulationSpeed = 0; // 50 is a good normal value
    protected float bypassAngleAccumulator = 0;

    protected virtual void Start()
    {
        if (rotateAtStart) RotateInstantlyTowardsTarget();
    }

    public override EnemySteering GetSteering()
    {
        EnemySteering steering = new EnemySteering();

        if (isActive)
        {
            if (bypassAngleAccumulationSpeed != 0) AccumulateBypassAngle();

            float rotation = targetOrientation - agent.orientation;
            rotation = MapToRange(rotation);
            float rotationSize = Mathf.Abs(rotation);
            if (rotationSize < targetRadius)
                return steering;
            float targetRotation = agent.maxRotation;
            if (rotationSize < slowRadius)
                targetRotation = agent.maxRotation * rotationSize / slowRadius;
            targetRotation *= Mathf.Sign(rotation);
            steering.angular = targetRotation - agent.rotation;
            steering.angular /= timeToTarget;
            float angularAccel = Mathf.Abs(steering.angular);
            if (angularAccel > agent.maxAngularAccel)
            {
                steering.angular /= angularAccel;
                steering.angular *= agent.maxAngularAccel;
            }
        }

        return steering;
    }

    private void AccumulateBypassAngle()
    {
        var direction = target.transform.position - transform.position;
        //targetOrientation = Mathf.Atan2(direction.x, direction.y);
        //targetOrientation *= Mathf.Rad2Deg;

        var hits = RaycastHits(direction);
        hits = (from t in hits
                where t.transform.gameObject.tag == "Environment" || t.transform.gameObject.tag == "Player"
                select t).ToArray();
        var status = hits.Length != 0 && hits[0].transform.gameObject.tag != "Player" ? "Found wall" : "Wall not found, " + hits.Length;
        if (hits.Length != 0 && hits[0].transform.gameObject.tag != "Player")
        {
            bypassAngleAccumulator += Mathf.Sign(bypassAngleAccumulator + targetOrientation / 2) * bypassAngleAccumulationSpeed * Time.deltaTime;
        }
        else
        {
            bypassAngleAccumulator -= Mathf.Sign(bypassAngleAccumulator) * bypassAngleAccumulationSpeed * Time.deltaTime * 2f;
        }
        targetOrientation += bypassAngleAccumulator;
        // print(status + ": " + bypassAngleAccumulator + " -> " + targetOrientation);
    }

    private RaycastHit2D[] RaycastHits(Vector2 direction)
    {
        //Debug.DrawLine(transform.position, direction1.normalized);
        return Physics2D.RaycastAll(transform.position, new Vector2(transform.up.x, transform.up.y) * 0.5f + direction.normalized, maxBypassRaycastDistance);
    }

    protected float targetOrientation;
}
