using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    protected float EnemySpeed = 2f;
    protected GameObject Player;
    protected SpriteRenderer sprite;
    protected MonsterLife lifeComp;

    protected virtual void Start()
    {
        lifeComp = GetComponent<MonsterLife>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void MoveAndRotate()
    {
        if (allowMovement)
        {
            if (lifeComp.FadeInLeft == 0) {
                MoveToward();
            }
            Rotation();
        }
    }

    protected void Update()
    {
        if (Pause.Paused && !wasPausedLastFrame)
        {
            wasPausedLastFrame = true;
            OnPauseGame();
        }

        if (Pause.UnPaused)
        {
            if (wasPausedLastFrame)
            {
                wasPausedLastFrame = false;
                OnResumeGame();
            }

            UpdateEnemy();
        }
    }

    protected virtual void UpdateEnemy()
    {
        MoveAndRotate();
    }

    protected virtual void MoveToward()
    {      
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, EnemySpeed * Time.deltaTime);
    }

    protected virtual void Rotation()
    {
        float z = Mathf.Atan2((Player.transform.position.y - transform.position.y), (Player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0, 0, z);
    }

    public void StopMovement(float time)
    {
        allowMovement = false;
        StartCoroutine(EnableMovement(time));
    }

    private IEnumerator EnableMovement(float wait)
    {
        yield return new WaitForSeconds(wait);
        
        allowMovement = true;
    }

    void OnPauseGame()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        savedVelocity = rigidbody.velocity;
        rigidbody.isKinematic = true;
        rigidbody.Sleep();
    }

    void OnResumeGame()
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.WakeUp();
        rigidbody.isKinematic = false;
        rigidbody.AddForce(savedVelocity, ForceMode2D.Impulse);
    }

    Vector3 savedVelocity = new Vector3();
    private bool wasPausedLastFrame = false;

    private bool allowMovement = true;
}
