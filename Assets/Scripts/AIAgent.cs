using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public float orientation;
    public float rotation;
    public Vector2 velocity;
    protected EnemySteering steering;


    private void Start()
    {
        velocity = Vector2.zero;
        steering = new EnemySteering();
    }

    public void SetSteering(EnemySteering steering, float weight)
    {
        this.steering.linear += steering.linear * weight;
        this.steering.angular += steering.angular * weight;
    }

    public virtual void Update()
    {
        Vector2 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        while (orientation < 0.0f)
        {
            orientation += 360.0f;
        }
        while (orientation > 360.0f)
        {
            orientation -= 360.0f;
        }
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.back, orientation);
    }

    public virtual void LateUpdate()
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        steering = new EnemySteering();
    }

    private Dictionary<int, List<EnemySteering>> groups;
}
