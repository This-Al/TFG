using System.Collections;
using System.Collections.Generic;
//using System.Numerics;

//using System.Numerics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    public float speed;
    private Rigidbody2D playerRigidbody;
    public GameObject bulletPrefab;
    public SpriteRenderer playerSprite;
    public Collider2D playerCollider;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    private float shootHor;
    private float shootVer;

    public GameObject captureLinePrefab;
    private GameObject captureLine;
    private float trailCharge;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        trailCharge = GameController.TrailCharge;
        
        float horizontal = Input.GetAxisRaw("Horizontal"); //GetAxisRaw removes the fake smooth input, so it doesnt decelerate
        float vertical = Input.GetAxisRaw("Vertical");

        shootHor = Input.GetAxis("ShootHorizontal");
        shootVer = Input.GetAxis("ShootVertical");

        if ((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVer);
            lastFire = Time.time;
        }

        Vector3 velocityVector = new Vector3(horizontal * speed, vertical * speed, 0);
        velocityVector.Normalize();
        playerRigidbody.velocity = velocityVector * speed;
        flipSprite();
        animator.SetFloat("Speed", Mathf.Abs(playerRigidbody.velocity.magnitude));

        if(Input.GetKeyDown("space"))
        {
            Paint();
        }

        if(Input.GetKeyUp("space"))
        {
            Destroy(captureLine);
        }
        
    }

    void flipSprite()
    {
        if(playerRigidbody.velocity.x > 0f) 
        {
            playerSprite.flipX = false;
        };
        if(playerRigidbody.velocity.x < 0f)
        {
            playerSprite.flipX = true;
        }
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
                (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
                (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
                0
        );
    }

    void Paint()
    {
        captureLine = Instantiate(captureLinePrefab, transform);
    }

}