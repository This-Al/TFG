using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour
{
    public static GameControler instance;

    private int health;
    private int maxHealth;
    private float moveSpeed;
    private float fireRate;

    public static int Health{ get => health; set => Health = value; }
    public static int MaxHealth{ get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed{ get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate{ get => fireRate; set => fireRate = value; }


    // Start is called before the first frame update
    private void Awake()        //singleton
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
