using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Door[] doors;
    public Dictionary<Direction.Side, Door> doorsSided = new Dictionary<Direction.Side, Door>();

    [HideInInspector]
    private Labirint labirint = null;
    public int roomID = -1; // -1 for not set

    public enum RoomType {empty,arena }
    public RoomType roomType;

    public Transform possibleContainerPosition;

    [HideInInspector]
    public MonsterManager monsterManager;

    private void Awake()
    {
        labirint = Labirint.instance;
        if (possibleContainerPosition == null) possibleContainerPosition = transform; // if forgot to set, center of room
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
                if (door.direction == Direction.Side.UNSET && door.directionAutoset())
                     Debug.LogError("Door direction was not set");
                else doorsSided[door.direction] = door;
                if (door.room == null) door.room = this;
            }
        }
    }

    public void MoveToRoom(Door wayInDoor) {
        wayInDoor.connectedDoor.room.LeaveRoom();
        CameraForLabirint.instance.ChangeRoom(wayInDoor.room.gameObject);
        GameObject.FindGameObjectWithTag("Player").transform.position = wayInDoor.transform.position;
        Labirint.instance.OnRoomChanged(roomID);
        ArenaInitCheck();
        LightCheck();
    }

    public void ArenaInitCheck()
    {
        if (roomType == RoomType.arena)
        {            
            if (!Labirint.instance.blueprints[roomID].visited) 
            {
                if(GetComponent<ArenaEnemySpawner>()!=null)
                    GetComponent<ArenaEnemySpawner>().enabled = true;
                if (GetComponent<MonsterManager>() != null)
                    GetComponent<MonsterManager>().spawnAvailable = true;
                LockRoom();
            }
            else {

                if (GetComponent<ArenaEnemySpawner>() != null)
                    GetComponent<ArenaEnemySpawner>().KillThemAll();
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
            GetComponent<ArenaEnemySpawner>()?.KillThemAll();
        }
        Labirint.instance.blueprints[roomID].visited = true;
    }

    public void LightCheck() {
        if (monsterManager != null)
            if (roomType == RoomType.arena && !labirint.blueprints[roomID].visited)
                monsterManager.roomLighting.labirintRoomEnterDark();
            else
                monsterManager.roomLighting.labirintRoomEnterBright();
        else
            GetComponent<RoomLighting>().labirintRoomEnterBright(); // исключение для комнат баз монстров

    }
}
