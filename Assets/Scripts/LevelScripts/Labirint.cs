using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlueprint
{
    public int UpRoom = -1; // index for array
    public int DownRoom = -1;// -1 for no room, to prevent auto link to room #0
    public int LeftRoom = -1;
    public int RightRoom = -1;

    public GameObject instance; // link to room if it is spawned
    public GameObject prefab;

    public bool visited = false;
}

public class Labirint : MonoBehaviour
{
    public GameObject[] RoomPrefabs;//from inspector 
    public RoomBlueprint[] blueprints; 
    private List<int> activeRooms = new List<int>();
    public int currentRoomID = 0;
    private const float distanceToNewDoor = 10f; // distance from old door no new door, defines distance between rooms
    static public Labirint instance;
    private Vector3 respawnPoint;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitBlueprints();
        StartingRoomSpawn();        
    }

    private void InitBlueprints()
    {
        blueprints = new RoomBlueprint[4];
        for (int i = 0; i<4; i++) { 
            blueprints[i] = new RoomBlueprint();
            blueprints[i].prefab = RoomPrefabs[i];
        }

        blueprints[0].RightRoom = 1; // хардкод для связей между комнатами
        blueprints[1].LeftRoom = 0;

        blueprints[1].RightRoom = 2;
        blueprints[2].LeftRoom = 1;

        blueprints[1].UpRoom = 3;
        blueprints[3].DownRoom = 1;

        //         [3]
        //          |
        //map: [0]-[1]-[2]- -> bossScene
    }

    void StartingRoomSpawn() {
        if (GameObject.FindGameObjectWithTag("Room") == null)
        {
            SpawnRoom(0);
            OnRoomChanged(0);
            blueprints[0].instance.GetComponent<Room>().ArenaInitCheck();
        }
        else { // for start from choisen room, add prefab, set roomID, and connected room will be spawned
            Room startingRoom = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
            if (startingRoom.roomID > -1 && startingRoom.roomID < blueprints.Length+1)
            { // only if room id was set                
                activeRooms.Add(startingRoom.roomID);
                blueprints[startingRoom.roomID].instance = startingRoom.gameObject;
                blueprints[startingRoom.roomID].instance.GetComponent<Room>().ArenaInitCheck();
                OnRoomChanged(startingRoom.roomID);
                GetComponent<CameraForLabirint>().ChangeRoom(startingRoom.gameObject);
                GameObject.FindWithTag("Player").transform.position = startingRoom.transform.position;
            }
        }
    }

    public void OnRoomChanged(int roomIndex){ // spawn neighbors and destroy not neighbor rooms after transition to new room
        currentRoomID = roomIndex;
        List<int> roomsToActivate = new List<int>(); // list of rooms wich should be present after this method 
        roomsToActivate.Add(currentRoomID);
        if (blueprints[currentRoomID].UpRoom != -1)        
            roomsToActivate.Add(blueprints[currentRoomID].UpRoom);
        if (blueprints[currentRoomID].DownRoom != -1)
            roomsToActivate.Add(blueprints[currentRoomID].DownRoom);
        if (blueprints[currentRoomID].LeftRoom != -1)
            roomsToActivate.Add(blueprints[currentRoomID].LeftRoom);
        if (blueprints[currentRoomID].RightRoom != -1)
            roomsToActivate.Add(blueprints[currentRoomID].RightRoom);

        //destroy rooms who are not neighbirs
        List<int> toDestroy = new List<int>();  
        foreach (int roomID in activeRooms) {
            if (!roomsToActivate.Contains(roomID)) 
            {
                blueprints[roomID].instance.GetComponent<Room>().DisconnectRoom();
                Destroy(blueprints[roomID].instance);
                toDestroy.Add(roomID);
            }
        }
        foreach (int roomID in toDestroy) { // because cant remove from list in foreach of same list
            activeRooms.Remove(roomID);
        }

        // add rooms who neighbors and not spawned earlier
        foreach (int roomID in roomsToActivate) {
            if (!activeRooms.Contains(roomID))
            {
                SpawnRoom(roomID);
                Room currentRoom = blueprints[currentRoomID].instance.GetComponent<Room>();
                Room newRoom = blueprints[roomID].instance.GetComponent<Room>();
                Door oldDoor = null;
                Door newDoor = null;
                Vector3 offset = Vector3.zero;
                if (blueprints[currentRoomID].UpRoom == roomID) {
                    oldDoor = currentRoom.upDoor;
                    newDoor = newRoom.downDoor;
                    offset = Vector3.up * distanceToNewDoor; // vector between doors;
                } else if (blueprints[currentRoomID].DownRoom == roomID){
                    oldDoor = currentRoom.downDoor;
                    newDoor = newRoom.upDoor;
                    offset = Vector3.down * distanceToNewDoor;
                } else if (blueprints[currentRoomID].LeftRoom == roomID) {
                    oldDoor = currentRoom.leftDoor;
                    newDoor = newRoom.rightDoor;
                    offset = Vector3.left * distanceToNewDoor;
                } else if (blueprints[currentRoomID].RightRoom == roomID){
                    oldDoor = currentRoom.rightDoor;
                    newDoor = newRoom.leftDoor;
                    offset = Vector3.right * distanceToNewDoor;
                }
                ConnectDoors(oldDoor, newDoor);
                offset = oldDoor.transform.localPosition + offset - newDoor.transform.localPosition; // between rooms
                newRoom.transform.position = currentRoom.transform.position + offset;

                if (blueprints[roomID].instance.GetComponent<ArenaEnemySpawner>() != null && roomID != currentRoomID) { //if room with arena, but we are not in it yet
                    blueprints[roomID].instance.GetComponent<ArenaEnemySpawner>().enabled = false;
                }
            }
        }
        CameraForLabirint.instance.ChangeRoom(blueprints[currentRoomID].instance);
        respawnPoint = GameObject.FindWithTag("Player").transform.position;
    }

    void ConnectDoors(Door door1, Door door2) {
        door1.connectedDoor = door2;
        door2.connectedDoor = door1;
    }

    void SpawnRoom(int id) {
        activeRooms.Add(id);
        blueprints[id].instance = (GameObject)Instantiate(blueprints[id].prefab, Vector3.zero, Quaternion.identity); // zero position to move prefab under player
        blueprints[id].instance.GetComponent<Room>().roomID = id;
        blueprints[id].instance.GetComponent<Room>().DoorsInit();
    }

    public void ReloadRoom() {
        Vector3 savedPosition = blueprints[currentRoomID].instance.transform.position;
        blueprints[currentRoomID].instance.GetComponent<ArenaEnemySpawner>()?.KillThemAll();
        blueprints[currentRoomID].instance.GetComponent<Room>().DisconnectRoom();
        Destroy(blueprints[currentRoomID].instance);
        SpawnRoom(currentRoomID);
        blueprints[currentRoomID].instance.transform.position = savedPosition;

        if (blueprints[currentRoomID].UpRoom > -1) {
            ConnectDoors(blueprints[currentRoomID].instance.GetComponent<Room>().upDoor, blueprints[blueprints[currentRoomID].UpRoom].instance.GetComponent<Room>().downDoor);
        }
        if (blueprints[currentRoomID].DownRoom > -1) {
            ConnectDoors(blueprints[currentRoomID].instance.GetComponent<Room>().downDoor, blueprints[blueprints[currentRoomID].DownRoom].instance.GetComponent<Room>().upDoor);
        }
        if (blueprints[currentRoomID].LeftRoom > -1) { 
            ConnectDoors(blueprints[currentRoomID].instance.GetComponent<Room>().leftDoor, blueprints[blueprints[currentRoomID].LeftRoom].instance.GetComponent<Room>().rightDoor);
        }
        if (blueprints[currentRoomID].RightRoom > -1) {
            ConnectDoors(blueprints[currentRoomID].instance.GetComponent<Room>().rightDoor, blueprints[blueprints[currentRoomID].RightRoom].instance.GetComponent<Room>().leftDoor);
        }
        
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = respawnPoint;
    }
}
