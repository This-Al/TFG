using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;
    public GameObject doorClosed;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Vector3 targetOffset = new Vector2(0, 0);

            switch(doorType)
                {
                    case Door.DoorType.left:
                    targetOffset = new Vector2(-1.5f, 0);
                    break;
                    case Door.DoorType.right:
                    targetOffset = new Vector2(1.5f, 0);
                    break;
                    case Door.DoorType.top:
                    targetOffset = new Vector2(0, 1.5f);
                    break;
                    case Door.DoorType.bottom:
                    targetOffset = new Vector2(0, -1.5f);
                    break;
                }

            Vector2 targetPosition = player.transform.position + targetOffset;    
            player.transform.position = targetPosition;   
        }
    }
}
