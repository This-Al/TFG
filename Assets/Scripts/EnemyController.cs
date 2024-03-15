using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,
    Follow,
    Attack,
    Die,
    Idle
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
            case(EnemyState.Wander):
                //Wander();
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
            case(EnemyState.Idle):
                Idle();
            break;
        }

        if(IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }
        else if(!IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Idle; //prev Wander
        }
        if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currState = EnemyState.Attack;
        }

        
        flipSprite();
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    // private IEnumerator ChooseDirection()
    // {
    //     chooseDir = true;
    //     yield return new WaitForSeconds(Random.Range(2f, 8f));                                          //chooses random direction
    //     randomDir = new Vector3(0, 0, Random.Range(0, 360));                                            //chooses rotation randomly
    //     Quaternion nextRotation = Quaternion.Euler(randomDir);                                          //rotates enemy
    //     transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
    //     chooseDir = false;
    // }

    // void Wander()
    // {
    //     if(!chooseDir)
    //     {
    //         StartCoroutine(ChooseDirection());
    //     }

    //     transform.position += -transform.right * speed * Time.deltaTime;
    //     if(IsPlayerInRange(range))
    //     {
    //         currState = EnemyState.Follow;
    //     }
    // }

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
        Destroy(gameObject);
    }
}
