using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [HideInInspector] public MonsterManager monsterManager;
    [HideInInspector] public List<MonsterRoomModifier> externalMRMods = new List<MonsterRoomModifier>();

    private void Awake()
    {
        labirint = Labirint.instance;
        externalMRMods = labirint.commonMRMods;
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
                    GetComponent<MonsterManager>().UnfreezeMonsters();
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
                monsterManager.roomLighting.LabirintRoomEnterDark(monsterManager.EnemyCount());
            else
                monsterManager.roomLighting.LabirintRoomEnterBright();
        else
            GetComponent<RoomLighting>().LabirintRoomEnterBright(); // exception for room without monsters

    }
    
    public Dictionary<Direction.Side, float> GetBordersFromTilemap() {
        Dictionary<Direction.Side, float> result = new Dictionary<Direction.Side, float>();
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
        float left, right, up, down;
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.tag == "Environment")
            { // to separete layer with walls
                Vector3Int tilePosition;
                left = Mathf.Infinity;
                right = -Mathf.Infinity;
                up = -Mathf.Infinity;
                down = Mathf.Infinity;
                for (int x = tilemap.origin.x; x < tilemap.size.x; x++)
                {
                    for (int y = tilemap.origin.y; y < tilemap.size.y; y++)
                    {
                        tilePosition = new Vector3Int(x, y, 0);
                        if (tilemap.HasTile(tilePosition))
                        {
                            if (tilemap.CellToWorld(tilePosition).x < left) left = tilemap.CellToWorld(tilePosition).x;
                            if (tilemap.CellToWorld(tilePosition).x > right) right = tilemap.CellToWorld(tilePosition).x;
                            if (tilemap.CellToWorld(tilePosition).y < down) down = tilemap.CellToWorld(tilePosition).y;
                            if (tilemap.CellToWorld(tilePosition).y > up) up = tilemap.CellToWorld(tilePosition).y;
                        }
                    }
                }
                result[Direction.Side.LEFT] = left + 1.5f; //mb need to replace with something tile size related later
                result[Direction.Side.RIGHT] = right + 0.5f;
                result[Direction.Side.UP] = up + 0.5f;
                result[Direction.Side.DOWN] = down + 1.5f;                
            }
        }
        return result;
    }

    public bool RectIsInbounds(float x, float y, float sizeX, float sizeY) {
        bool result = true;
        Dictionary<Direction.Side, float> bounds = GetBordersFromTilemap();
        result = x > bounds[Direction.Side.LEFT] &&
            x + sizeX < bounds[Direction.Side.RIGHT] &&
            y > bounds[Direction.Side.DOWN] &&
            y + sizeY < bounds[Direction.Side.UP];
        return result;
    }

}
