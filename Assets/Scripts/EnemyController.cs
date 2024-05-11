using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Follow,
    Attack,
    Die
};

public enum EnemyType
{
    Melee,
    Ranged
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float range;
    public float speed;
    public float attackRange;
    public float bulletSpeed;
    public float cooldown;
    private bool chooseDir = false;
    private bool dead = false;
    private bool cooldownAttack = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    public SpriteRenderer enemySprite;
    public Animator animator;
    public bool isInRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        switch(currState)
        {
            case(EnemyState.Idle):
                Idle();
            break;
            case(EnemyState.Follow):
                Follow();
            break;
            case(EnemyState.Attack):
                Attack();
            break;
            case(EnemyState.Die):
                //Die();
            break;
        }

        if(isInRoom)
        {
            if(IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;
            }
            else if(!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Idle;
            }
            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
        
        flipSprite();
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }


    void flipSprite()
    {
        if(transform.position.x > player.transform.position.x) //player is at the left
        {
            enemySprite.flipX = false;
        };
        if(transform.position.x < player.transform.position.x) //player is at the right
        {
            enemySprite.flipX = true;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void Attack()
    {
        if(!cooldownAttack)
        {
            switch(enemyType)
            {
                case(EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(Cooldown());
                break;
                case(EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(Cooldown());
                break;
            }
        }
    }

    void Idle()
    {
        if(IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    private IEnumerator Cooldown()
    {
        cooldownAttack = true;
        yield return new WaitForSeconds(cooldown);
        cooldownAttack = false;
    }

    public void Death()
    {
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());  
        Destroy(gameObject);
    }

}
