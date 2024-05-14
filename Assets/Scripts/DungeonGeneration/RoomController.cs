using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Tilemaps;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}


public class RoomController : MonoBehaviour
{
    public static RoomController instance;
    string currentWorldName = "FirstLevel";
    RoomInfo currentLoadRoomData;
    public Room currRoom;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;
    bool isBossRoomSpawned = false;
    bool updatedRooms = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // LoadRoom("Start", 0, 0);
        // LoadRoom("Empty", 1, 0);
        // LoadRoom("Empty", -1, 0);
        // LoadRoom("Empty", 0, 1);
        // LoadRoom("Empty", 0, -1);
    }

    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if(isLoadingRoom)
        {
            return;
        }
        
        if(loadRoomQueue.Count == 0)
        {
            if(!isBossRoomSpawned)
            {
                StartCoroutine(SpawnBossRoom());
            } 
            else if (isBossRoomSpawned && !updatedRooms)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        isBossRoomSpawned = true;
        yield return new WaitForSeconds(1f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void LoadRoom ( string name, int x, int y ) //x, y in int matrix
    {
        if(DoesRoomExist(x, y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive); //this will make the scenes overlap

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom( Room room ) //set room in position within Scene
    {
        if(!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                    currentLoadRoomData.X * room.Width,
                    currentLoadRoomData.Y * room.Height,
                    0 
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject); //room already in existence, so no use for this
            isLoadingRoom = false; //remove room from loaded rooms
        }
    }

    public bool DoesRoomExist( int x, int y )
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom( int x, int y )
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y);
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;

        StartCoroutine(RoomCoroutine());
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateRooms();
    }

    public void UpdateRooms() //closes and opens doors and activates enemies 
    {
        foreach(Room room in loadedRooms)
        {
            if(currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>(); 
                if(enemies != null)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.isInRoom = false;
                    }

                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.GetComponent<EdgeCollider2D>().enabled = false; 
                        //door.doorClosed.SetActive(false); 
                    }
                }
                else
                {
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.GetComponent<EdgeCollider2D>().enabled = true; 
                        door.doorClosed.SetActive(false); 
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies.Length > 0)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.isInRoom = true;
                    }
                    
                    foreach(Door door in room.GetComponentsInChildren<Door>()) //close doors
                    {
                        door.GetComponent<EdgeCollider2D>().enabled = false; 
                        door.doorClosed.SetActive(true);
                    }
                }
                else
                {
                    foreach(Door door in room.GetComponentsInChildren<Door>()) //open doors
                    {
                        door.GetComponent<EdgeCollider2D>().enabled = true;
                        if(door.doorClosed != null) 
                        {
                            door.doorClosed.SetActive(false);
                        } 

                    }
                }  
            }
        }
    }
}
