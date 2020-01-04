using UnityEngine;

public class Teleport : TimedAttack
{
    private float CoolDownBefore;
    [SerializeField]
    private float Scatter = 8f;

    private ArenaEnemySpawner arena;
    
    private float maxspeedSaved = 0f; //to hold maxSpeed when monster is stopped
    private bool shakeMode = false;
    public float shakeAmp = 0.1f; // multiplier to shake distance

    protected override void Awake() 
    {
        base.Awake();
        arena = GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<ArenaEnemySpawner>();
        maxspeedSaved = agent.maxSpeed;
    }

    protected override void CompleteAttack()
    {
        CoolDownBefore = Mathf.Max(CoolDownBefore - Time.deltaTime, 0);
        if (CoolDownBefore == 0)
        {
            int i = 0;
            while (i < 5)
            {
                i++;
                float Xpos = Random.Range(-100, 100);
                float YPos = Random.Range(-100, 100);
                var vect = new Vector2(target.transform.position.x - Xpos, target.transform.position.y - YPos);
                vect.Normalize();
                vect *= Scatter;
                Vector3 NVector = new Vector3(vect.x, vect.y);
                if (arena.RoomBounds.x > Mathf.Abs(target.transform.position.x + NVector.x) &&
                    arena.RoomBounds.y > Mathf.Abs(target.transform.position.y + NVector.y))
                {
                    // We need teleport position not to be a solid object
                    if (Physics2D.OverlapCircle(target.transform.position + NVector, 2.5f, LayerMask.GetMask("Solid")) == null)
                    {
                        var audio = GetComponent<AudioSource>();
                        AudioManager.Play("Blink", audio);
                        transform.position = target.transform.position + NVector;
                        GetComponent<MonsterLife>().FadeIn(0.5f);
                        StopKnockback();
                        EndShake();
                        break;
                    }
                }
            }
            if (i == 5) EndShake();//in case we can't find spot for teleport
        }
    }

    private void EndShake() {
        agent.maxSpeed = maxspeedSaved;
        shakeMode = false;
    }

    protected override void AttackAnimation()
    {     
        agent.maxSpeed = 0f;
        shakeMode = true;
    }

    public override void CalledUpdate()
    {
        base.CalledUpdate();
        if (shakeMode) {
            Vector2 shift = new Vector2(Random.Range(-shakeAmp, shakeAmp), Random.Range(-shakeAmp, shakeAmp));
            gameObject.transform.Translate(shift,Space.World);
        }
    }

    private void StopKnockback()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        rigidbody.isKinematic = false;
    }
}
