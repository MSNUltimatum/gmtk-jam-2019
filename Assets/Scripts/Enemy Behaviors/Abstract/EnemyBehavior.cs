using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    public float weight = 1.0f;
    public GameObject target;
    protected Agent agent;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void Update()
    {
        agent.SetSteering(GetSteering(), weight);
    }

    public abstract EnemySteering GetSteering();

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