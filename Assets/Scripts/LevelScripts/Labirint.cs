using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBlueprint
{
    public Dictionary<Direction.Side, int> rooms = new Dictionary<Direction.Side, int>();

    public GameObject instance; // link to room if it is spawned
    public GameObject prefab;

    public bool visited = false;
    public string exitSceneName = ""; // not empty only for exit room

    public GameObject contanerPrefab = null;
    public bool containerWasOpened = false;
}

public class Labirint : MonoBehaviour
{
    //public GameObject[] RoomPrefabs;//from inspector 
    public RoomBlueprint[] blueprints; 
    private List<int> activeRooms = new List<int>();
    public int currentRoomID = 0;
    private const float distanceToNewDoor = 10f; // distance from old door no new door, defines distance between rooms
    static public Labirint instance;
    private Vector3 respawnPoint;
    public int difficultySetting = 1;
    public List<MonsterRoomModifier> commonMRMods;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        instance = this;
        DifficultyLoad();
        LabirintBuilder builder = GetComponent<LabirintBuilder>();
        if (builder == null)
        {
            //InitBlueprints();
            Debug.LogError("Cant find labirint builder script");
        } else {
            builder.BuildLabirint(this);
        }
        StartingRoomSpawn();
    }


//    private void InitBlueprints()
//    {
//        int arraySize = RoomPrefabs.Length;
//        blueprints = new RoomBlueprint[arraySize];
//        for (int i = 0; i < arraySize; i++)
//        {
//            blueprints[i] = new RoomBlueprint();
//            blueprints[i].prefab = RoomPrefabs[i];
//        }        
//        HardcodeLabirintConstruction();
//    }

    public void InitBlueprintsFromBuilder() {
        for (int i = 0; i < blueprints.Length; i++)
        {
            blueprints[i] = new RoomBlueprint();
        }
    }

    private void HardcodeLabirintConstruction()
    {

        blueprints[0].rooms[Direction.Side.RIGHT] = 1; // хардкод для связей между комнатами
        blueprints[1].rooms[Direction.Side.LEFT] = 0;

        blueprints[1].rooms[Direction.Side.RIGHT] = 2;
        blueprints[2].rooms[Direction.Side.LEFT] = 1;

        blueprints[1].rooms[Direction.Side.UP] = 3;
        blueprints[3].rooms[Direction.Side.DOWN] = 1;

        //         [3]
        //          |
        //map: [0]-[1]-[2]- -> bossScene
    }

    void StartingRoomSpawn() {
        if (GameObject.FindGameObjectWithTag("Room") == null) {
            SpawnRoom(0);
            OnRoomChanged(0);
            blueprints[0].instance.GetComponent<Room>().ArenaInitCheck();
            blueprints[0].instance.GetComponent<Room>().LightCheck();
        }
        else { // for start from choisen room, add prefab, set roomID, and connected room will be spawned
            Room startingRoom = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
            if (startingRoom.roomID > -1 && startingRoom.roomID < blueprints.Length+1)
            { // only if room id was set                
                if (startingRoom.name == blueprints[startingRoom.roomID].prefab.name) {
                    activeRooms.Add(startingRoom.roomID);
                    startingRoom.DoorsInit();
                    blueprints[startingRoom.roomID].instance = startingRoom.gameObject;
                    blueprints[startingRoom.roomID].instance.GetComponent<Room>().ArenaInitCheck();
                    OnRoomChanged(startingRoom.roomID);
                    GetComponent<CameraForLabirint>().ChangeRoom(startingRoom.gameObject);
                    GameObject.FindWithTag("Player").transform.position = startingRoom.transform.position;
                }
                else
                {
                    Debug.Log("Starting room ID mismatch");
                    GameObject.FindWithTag("Player").transform.position = startingRoom.transform.position;
                    GetComponent<CameraForLabirint>().ChangeRoom(startingRoom.gameObject);
                }
            }
        }
    }

    public void OnRoomChanged(int roomIndex){ // spawn neighbors and destroy not neighbor rooms after transition to new room
        currentRoomID = roomIndex;
        List<int> roomsToActivate = new List<int>(); // list of rooms wich should be present after this method 
        roomsToActivate.Add(currentRoomID);
        
        foreach (var side in Direction.sides)
        {
            if (blueprints[currentRoomID].rooms.ContainsKey(side))
            {
                roomsToActivate.Add(blueprints[currentRoomID].rooms[side]);
            }
        }

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
                foreach (var side in Direction.sides)
                {
                    if (blueprints[currentRoomID].rooms.ContainsKey(side) && blueprints[currentRoomID].rooms[side] == roomID)
                    {
                        oldDoor = currentRoom.doorsSided[side];
                        newDoor = newRoom.doorsSided[Direction.InvertSide(side)];
                        oldDoor.SpawnDoor();
                        newDoor.SpawnDoor();
                        offset = Direction.SideToVector3(side) * distanceToNewDoor;
                    }
                }
                ConnectDoors(oldDoor, newDoor);
                offset = oldDoor.transform.localPosition + offset - newDoor.transform.localPosition; // between rooms
                newRoom.transform.position = currentRoom.transform.position + offset;

                if (blueprints[roomID].instance.GetComponent<ArenaEnemySpawner>() != null && roomID != currentRoomID) {
                    //if room with arena, but we are not in it yet
                    blueprints[roomID].instance.GetComponent<ArenaEnemySpawner>().enabled = false;
                }
            }
        }
        CameraForLabirint.instance.ChangeRoom(blueprints[currentRoomID].instance);
        respawnPoint = GameObject.FindWithTag("Player").transform.position;
        ExitCheck();
        ContainerCheck();
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

    public void ReloadRoom() { // сейчас не используется. делалось для перерождения игрока в этой же комнате
        Vector3 savedPosition = blueprints[currentRoomID].instance.transform.position;
        blueprints[currentRoomID].instance.GetComponent<ArenaEnemySpawner>()?.KillThemAll();
        blueprints[currentRoomID].instance.GetComponent<Room>().DisconnectRoom();
        Destroy(blueprints[currentRoomID].instance);
        SpawnRoom(currentRoomID);
        blueprints[currentRoomID].instance.transform.position = savedPosition;

        foreach (var side in Direction.sides)
        {
            if (blueprints[currentRoomID].rooms[side] > -1)
                ConnectDoors(
                    blueprints[currentRoomID].instance.GetComponent<Room>().doorsSided[side], 
                    blueprints[blueprints[currentRoomID].rooms[side]].instance.GetComponent<Room>().doorsSided[Direction.InvertSide(side)]);
        }
        
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = respawnPoint;
    }

    void ExitCheck() {
        if (blueprints[currentRoomID].exitSceneName != "")
        {
            foreach (Direction.Side side in Direction.sides)
            {
                if (blueprints[currentRoomID].rooms.ContainsKey(side))
                {
                    Door exitDoor = blueprints[currentRoomID].instance.GetComponent<Room>().doorsSided[Direction.InvertSide(side)];
                    exitDoor.SpawnDoor();
                    exitDoor.sceneName = blueprints[currentRoomID].exitSceneName;
                }
            }
        }
    }

    void ContainerCheck() {
        if (blueprints[currentRoomID].contanerPrefab != null && !blueprints[currentRoomID].containerWasOpened) {
            GameObject container = Instantiate(blueprints[currentRoomID].contanerPrefab, 
                blueprints[currentRoomID].instance.GetComponent<Room>().possibleContainerPosition.position, Quaternion.identity);
            container.transform.parent = blueprints[currentRoomID].instance.transform;
            container.GetComponent<Container>().blueprint = blueprints[currentRoomID];
        }
    }

    public static GameObject GetCurrentRoom() {
        return instance.blueprints[instance.currentRoomID].instance;
    }

    private void DifficultyLoad()
    {
        difficultySetting = PlayerPrefs.GetInt("Difficulty");
        if (difficultySetting == 1)
        {
            //Debug.Log("Normal mode loaded");
        }
        else if (difficultySetting == 2)
        {
            //Debug.Log("Hard mode loaded");
        }
        else
        {
            Debug.Log("Error on difficulty load, difficultySetting = " + difficultySetting.ToString());
            difficultySetting = 1; // to avoid errors on user side, better to load wrong difficulty than to crash
        }
    }
}
