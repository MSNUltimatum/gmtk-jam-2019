using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [HideInInspector]
    public Room room;
    [HideInInspector]
    public Door connectedDoor;
    [HideInInspector]
    public bool locked = false; 
    private GameObject player;

    [HideInInspector]
    public bool unlockOnTimer = false;
    private float timer = 1f;
    
    [SerializeField]
    public Direction.Side direction;

    public string sceneName=""; // name of scene to change on enter this door

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (unlockOnTimer && locked) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                Unlock();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // "Stay" needed to make door work if player was on trigger in moment of unlock
    {
        if (!locked && collision.gameObject == player) {
            if (sceneName == "") {
                connectedDoor.room.MoveToRoom(connectedDoor);
            } else {
                RelodScene.OnSceneChange?.Invoke();
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    public void Unlock() {
        if (locked)
        {
            locked = false;
            //animation?
        }
    }

    public void Lock()
    {
        locked = true;
        //animation?
        timer = 1f;
    }

    public bool directionAutoset() {
        if (sceneName == "")
        {
            if (transform.localPosition.x / (Mathf.Abs(transform.localPosition.y) + 0.001f) > 2)    // эврестическая оценка направления от центра на дверь, оверрайдится из инспектора
            {                                                                                       // если дверь на правой стене x/|y|>2 в локальных координатах то дверь правая, и т.п.
                direction = Direction.Side.RIGHT;
            }
            else if (transform.localPosition.x / (Mathf.Abs(transform.localPosition.y) + 0.001f) < -2) // + 0.001f - защита от деления на ноль
            {
                direction = Direction.Side.LEFT;
            }
            else if (transform.localPosition.y / (Mathf.Abs(transform.localPosition.x) + 0.001f) > 2)
            {
                direction = Direction.Side.UP;
            }
            else if (transform.localPosition.y / (Mathf.Abs(transform.localPosition.x) + 0.001f) < -2)
            {
                direction = Direction.Side.DOWN;
            }
            else
            {
                Debug.LogWarning("Cant get door dirrection automaticaly. Need to set it in inspector manually.");
                return false;
            }
            return true;
        }
        return false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // green box for trigger
        Gizmos.DrawWireCube(transform.position + (Vector3)GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size);

        BoxCollider2D blocker = null; // red box for blocker
        foreach (BoxCollider2D coll in GetComponentsInChildren<BoxCollider2D>()){
            if (coll.gameObject != gameObject) blocker = coll;
        }
        if (blocker != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(blocker.transform.position + (Vector3)blocker.offset, blocker.size);
        }

        if (connectedDoor != null) { // blue line for connection between doors
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, connectedDoor.transform.position);
        }

        if (locked)
        {
            Gizmos.color = Color.red; // red circle for locked door
            Gizmos.DrawWireSphere(transform.position, 1);
        }
        else
        {
            Gizmos.color = Color.green; // green circle for unlocked door, and spawn position
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
