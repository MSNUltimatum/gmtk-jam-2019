using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Door[] doors;

    [HideInInspector]
    public Door leftDoor = null;
    [HideInInspector]
    public Door rightDoor = null;
    [HideInInspector]
    public Door upDoor = null;
    [HideInInspector]
    public Door downDoor = null;

    [HideInInspector]
    private Labirint labirint = null;
    public int roomID = -1; // -1 for not set

    public enum RoomType {empty,arena }
    public RoomType roomType;
    
    private void Awake()
    {
        labirint = Labirint.instance;
    }

    private void Start()
    {
        DoorsInit();
    }

    public void DoorsInit() {
        doors = gameObject.GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            if (door.sceneName == "") {
                if (door.dirrection == Door.Dirrection.notSet)
                    door.dirrectionAutoset();
                switch (door.dirrection)
                {
                    case Door.Dirrection.left:
                        leftDoor = door;
                        break;
                    case Door.Dirrection.right:
                        rightDoor = door;
                        break;
                    case Door.Dirrection.up:
                        upDoor = door;
                        break;
                    case Door.Dirrection.down:
                        downDoor = door;
                        break;
                    default:
                        Debug.Log("error. Door direction was not set");
                        break;
                }
                if (door.room == null) door.room = this;
            }
        }
    }

    public void MoveToRoom(Door wayInDoor) {
        wayInDoor.connectedDoor.room.LeaveRoom();
        CameraForLabirint.instance.ChangeRoom(wayInDoor.room.gameObject);
        GameObject.FindGameObjectWithTag("Player").transform.position = wayInDoor.transform.position;
        labirint.OnRoomChanged(roomID);
        ArenaInitCheck();
    }

    public void ArenaInitCheck()
    {
        if (roomType == RoomType.arena)
        {
            if (!labirint.blueprints[roomID].visited) 
            {
                GetComponent<ArenaEnemySpawner>().enabled = true;
                LockRoom();
            }
            else {
                TimerUnlockRoom();
            }
        }
        else
        {
            TimerUnlockRoom();
        }
    }

    public void DisconnectRoom() // cutting door connections before destroy room, to avoid errors on Door.connectedDoor from neighors to destroyed room
    {
        foreach (Door door in doors) {
            if (door.connectedDoor != null) {
                door.connectedDoor.connectedDoor = null;
                door.connectedDoor = null;
            }
        }
    }

    public void UnlockRoom(){
        foreach (Door door in doors) {
            door.Unlock();
        }
    }

    public void LockRoom() {
        foreach (Door door in doors)
        {
            door.Lock();
        }
    }

    public void TimerUnlockRoom() {
        foreach (Door door in doors)
        {
            door.unlockOnTimer = true;
            door.Lock();
        }
    }

    public void LeaveRoom() {
        if (roomType == RoomType.arena)
        {
            GetComponent<ArenaEnemySpawner>().KillThemAll();
        }
        labirint.blueprints[roomID].visited = true;
    }
}
