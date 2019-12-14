using UnityEngine;

public class Teleport : EnemyBehavior
{
    [SerializeField]
    private Vector2 TpCooldownRange = new Vector2(3, 10);
    private float CoolDownBefore;
    [SerializeField]
    private float Scatter = 8f;

    private ArenaEnemySpawner arena;

    protected override void Awake()
    {
        base.Awake();
        CoolDownBefore = Random.Range(TpCooldownRange.x, TpCooldownRange.y);
        arena = GameObject.FindGameObjectWithTag("GameController")
            .GetComponent<ArenaEnemySpawner>();
    }

    public override void CalledUpdate()
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
                        break;
                    }
                }
            }
            CoolDownBefore = Random.Range(TpCooldownRange.x, TpCooldownRange.y);
        }
        base.CalledUpdate();
    }

    private void StopKnockback()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        rigidbody.isKinematic = false;
    }
}
