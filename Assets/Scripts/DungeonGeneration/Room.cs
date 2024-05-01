using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;

    private bool updatedDoors = false;

    public Room (int x, int y)
    {
        X = x; Y = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;
    public List<Door> doors = new List<Door>();

    public int enemyNumber = 0;

    public GameObject flyPrefab;

    public GameObject spawnPoint0;   
    public GameObject spawnPoint1;   
    public GameObject spawnPoint2;   
    public GameObject spawnPoint3;   
    public GameObject spawnPoint4;   

    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>(); //gets doors of the Room
        foreach(Door d in ds)
        {
            doors.Add(d);
            switch(d.doorType)
            {
                case Door.DoorType.left:
                leftDoor = d;
                break;
                case Door.DoorType.right:
                rightDoor = d;
                break;
                case Door.DoorType.top:
                topDoor = d;
                break;
                case Door.DoorType.bottom:
                bottomDoor = d;
                break;
            }
        }

        RoomController.instance.RegisterRoom(this);
        
        if(!(X == 0 && Y == 0))
        {
            StartCoroutine(EnemySpawn());
        }
    }

    void Update()
    {
        if(name.Contains("End") && !updatedDoors) 
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach(Door door in doors)
        {
            switch(door.doorType)
            {
                case Door.DoorType.left:
                    if(GetLeft() == null)
                        door.gameObject.SetActive(false);
                        doors.Remove(door);
                break;
                case Door.DoorType.right:
                    if(GetRight() == null)
                        door.gameObject.SetActive(false);
                        doors.Remove(door);
                break;
                case Door.DoorType.top:
                    if(GetTop() == null)
                        door.gameObject.SetActive(false);
                        doors.Remove(door);
                break;
                case Door.DoorType.bottom:
                    if(GetBottom() == null)
                        door.gameObject.SetActive(false);
                        doors.Remove(door);
                break;
            }
        }
    }

    // void OpenDoors()
    // {
    //     foreach(Door door in doors)
    //     {
    //         switch(door.doorType)
    //         {
    //             case Door.DoorType.left:
    //                 if(GetLeft() == null)
    //                     door.gameObject.SetActive(true);
    //             break;
    //             case Door.DoorType.right:
    //                 if(GetRight() == null)
    //                     door.gameObject.SetActive(true);
    //             break;
    //             case Door.DoorType.top:
    //                 if(GetTop() == null)
    //                     door.gameObject.SetActive(true);
    //             break;
    //             case Door.DoorType.bottom:
    //                 if(GetBottom() == null)
    //                     door.gameObject.SetActive(true);
    //             break;
    //         }
    //     }
    // }

    // void CloseDoors()
    // {
    //     foreach(Door door in doors)
    //     {
    //         door.gameObject.SetActive(false);
    //     }
    // }

    public Room GetLeft()
    {
        if(RoomController.instance.DoesRoomExist(X - 1, Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;
    }
    public Room GetRight()
    {
        if(RoomController.instance.DoesRoomExist(X + 1, Y))
        {
            return RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }
    public Room GetTop()
    {
        if(RoomController.instance.DoesRoomExist(X, Y + 1))
        {
            return RoomController.instance.FindRoom(X, Y + 1);
        }
        return null;
    }
    public Room GetBottom()
    {
        if(RoomController.instance.DoesRoomExist(X, Y - 1))
        {
            return RoomController.instance.FindRoom(X, Y - 1);
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3 ( X * Width, Y * Height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }

    IEnumerator EnemySpawn()
    {
        yield return null;

        GameObject currSpawnPoint;

        int enemyMaxNumber = Random.Range(1, 4);
        List<GameObject> spawnPointList = new List<GameObject>
        {
            spawnPoint0,
            spawnPoint1,
            spawnPoint2,
            spawnPoint3,
            spawnPoint4
        };
        
        int indexSpawnList;
        GameObject fly;

        while(enemyNumber < enemyMaxNumber)
        {
            indexSpawnList = Random.Range(0, spawnPointList.Count - 1);
            currSpawnPoint = spawnPointList[indexSpawnList];
            Instantiate(flyPrefab, currSpawnPoint.transform);
            enemyNumber++;
            spawnPointList.RemoveAt(indexSpawnList);
        }
    }

}
