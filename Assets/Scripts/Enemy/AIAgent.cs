using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public float maxSpeed = 3.5f;
    public float maxAccel = 100;
    public float maxRotation = 200f;
    public float maxAngularAccel = 10000f;
    public float velocityFallBackPower = 3f;
    public float orientation;
    public float rotation;
    public Vector2 velocity;
    protected EnemySteering steering;

    [Header("All Behaviours activation condition")]
    public List<ProximityCheckOption> proximityCheckOption = new List<ProximityCheckOption> { ProximityCheckOption.OnScreen, ProximityCheckOption.GroupAggroable };
    public float timeToLoseAggro = -1;

    public enum ProximityCheckOption
    {
        Distance,
        OnScreen,
        DirectSight,
        Always,
        GroupAggroable,
        ShootingAgroble
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
        steering = new EnemySteering();
        
        orientation = -transform.rotation.eulerAngles.z;
        rotation = 0;
    }

    public void SetSteering(EnemySteering steering, float weight)
    {
        this.steering.linear += steering.linear * weight;
        this.steering.angular += steering.angular * weight;
    }

    protected void FixedUpdate()
    {
        if (Pause.Paused) return;
        Vector2 velocityFallBack =
            velocity * velocityFallBackPower * Time.deltaTime;

        velocity -= velocityFallBack;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }

    protected virtual void Update()
    {
        ProceedPauseUnpause();
        if (Pause.Paused) return;

        if (!allowMovement) return;

        Vector2 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        
        orientation %= 360.0f;
        if (orientation < 0.0f)
        {
            orientation += 360.0f;
        }
        //rigidbody.velocity = velocity;
        //rigidbody.MovePosition(rigidbody.position + displacement);
        transform.rotation = Quaternion.Euler(0, 0, -orientation);

        var behaviors = GetComponents<EnemyBehavior>();
        foreach (var i in behaviors)
        {
            i.CalledUpdate();
        }

        rotation += Mathf.Max(steering.angular * Time.deltaTime);
        
        var speedUp = steering.linear * Time.deltaTime;
        if ((velocity + speedUp).magnitude < maxSpeed)
        {
            velocity += speedUp;
        }
        steering = new EnemySteering();
    }

    public void StopMovement(float time)
    {
        allowMovement = false;
        StartCoroutine(EnableMovement(time));
    }

    // TODO: Заменить на Event + Listener?
    private void ProceedPauseUnpause()
    {
        if (Pause.Paused && !wasPausedLastFrame)
        {
            wasPausedLastFrame = true;
            PauseKnockback();
        }

        if (Pause.UnPaused && wasPausedLastFrame)
        {
            wasPausedLastFrame = false;
            ResumeKnockback();
        }
    }

    private IEnumerator EnableMovement(float wait)
    {
        yield return new WaitForSeconds(wait);
        allowMovement = true;
    }

    public void PauseKnockback()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        savedVelocity = rigidbody.velocity;
        rigidbody.isKinematic = true;
        rigidbody.Sleep();
    }

    private void ResumeKnockback()
    {
        orientation = -transform.rotation.eulerAngles.z;
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.WakeUp();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(savedVelocity, ForceMode2D.Impulse);
    }

    Vector3 savedVelocity = new Vector3();
    private bool wasPausedLastFrame = false;
    private bool allowMovement = true;

    new private Rigidbody2D rigidbody;

    // private Dictionary<int, List<EnemySteering>> groups;
}
