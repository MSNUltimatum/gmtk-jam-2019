using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject target;
    protected Agent agent;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void Update()
    {
        agent.SetSteering(GetSteering());
    }

    public virtual EnemySteering GetSteering()
    {
        return new EnemySteering();
    }

    public float MapToRange(float rotation)
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
}