using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class EnemyBehavior : MonoBehaviour
{
    // public float priority = 1.0f;
    public float weight = 1.0f;
    protected GameObject target;
    protected bool isActive = false;
    protected AIAgent agent;

    [Header("Behaviour activation condition")]
    public List<ProximityCheckOption> proximityCheckOption = new List<ProximityCheckOption> { ProximityCheckOption.OnScreen };
    public float proximityCheckDistance = 19f;

    protected virtual void Awake()
    {
        agent = gameObject.GetComponent<AIAgent>();
        target = GameObject.FindGameObjectWithTag("Player");

        if (proximityCheckOption.Count == 0)
        {
            proximityCheckOption = new List<ProximityCheckOption> { ProximityCheckOption.OnScreen };
        }
    }

    public virtual void CalledUpdate()
    {
        if (!isActive && ProximityCheck())
        {
            isActive = true;
        }
        else if (isActive)
        {
            agent.SetSteering(GetSteering(), weight);
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public virtual EnemySteering GetSteering() {
        return new EnemySteering();
    }

    protected float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
                rotation += 360.0f;
            else
                rotation -= 360.0f;
        }
        return rotation;
    }

    private Camera currentCamera = null;
    protected virtual bool TargetOnScreen(GameObject target)
    {
        if (target == null) return false;
        if (currentCamera == null)
        {
            currentCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        var enemyInScreenSpace = currentCamera.WorldToViewportPoint(target.transform.position);
        return enemyInScreenSpace.x >= 0 && enemyInScreenSpace.x <= 1 && enemyInScreenSpace.y >= 0 && enemyInScreenSpace.y <= 1;
    }

    protected void RotateInstantlyTowardsTarget() {
        Vector2 direction = target.transform.position - transform.position;
        if (direction.magnitude > 0.0f)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.y);
            targetOrientation *= Mathf.Rad2Deg;
            targetOrientation += transform.localEulerAngles.z;
            transform.rotation = Quaternion.Euler(0, 0, -MapToRange(targetOrientation));
        }
    }

    protected bool ProximityCheck()
    {
        timeToProximityCheck = Mathf.Max(0, timeToProximityCheck - Time.deltaTime);
        if (timeToProximityCheck > 0) return false;
        timeToProximityCheck = proximityCheckPeriod;
        switch (proximityCheckOption[0])
        {
            case ProximityCheckOption.Distance:
                return Vector3.Distance(target.transform.position, transform.position) <= proximityCheckDistance;
            case ProximityCheckOption.DirectSight:
                // Check if raycast towards player hits player first and not environment
                var hits = (from t in Physics2D.RaycastAll(transform.position, target.transform.position - transform.position, proximityCheckDistance)
                           where t.transform.gameObject.tag == "Environment" || t.transform.gameObject.tag == "Player"
                           select t).ToArray();
                if (hits.Length == 0) return false;
                return hits[0].transform.CompareTag("Player");
            case ProximityCheckOption.Always:
                return true;
            case ProximityCheckOption.OnScreen:
                return TargetOnScreen(gameObject);
            default:
                Debug.LogError("Proximity check undefined condition");
                return false;
        }
    }

    public enum ProximityCheckOption
    {
        Distance,
        OnScreen,
        DirectSight,
        Always
    }

    private float proximityCheckPeriod = 0.5f;
    private float timeToProximityCheck = 0.5f;
}