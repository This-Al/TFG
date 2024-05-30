using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Follow,
    Attack,
    Die,
    Paralyze,
    Invincible
};

public enum BossType
{
    Melee,
    Ranged
};

public class BossController : MonoBehaviour
{
    GameObject player;
    public BossState currState = BossState.Idle;
    public BossType enemyType;
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
    public float paralyzedTime;
    public int bossHealth = 4;

    public bool isBoss = false;
    private Room bossRoom;
    
    public bool hasShield;
    public int playerDamage = 2;

    
    public AudioClip audioHit;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemySprite = GetComponent<SpriteRenderer>();
        bossRoom = GetComponentInParent<Room>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasShield)
        {
            enemySprite.color = new Color(0, 160, 255);
        } else 
        {
            enemySprite.color = new Color(255, 255, 255);
        }

        switch(currState)
        {
            case(BossState.Idle):
                Idle();
            break;
            case(BossState.Follow):
                Follow();
            break;
            case(BossState.Attack):
                Attack();
            break;
            case(BossState.Die):
                //Die();
            break;
            case(BossState.Paralyze):
                StartCoroutine(Paralyze());
            break;
            case(BossState.Invincible):
                Invincible();
            break;
        }

        if(isInRoom)
        {
            if(IsPlayerInRange(range) && currState != BossState.Die && currState != BossState.Paralyze && currState != BossState.Invincible)
            {
                currState = BossState.Follow;
            }
            else if(!IsPlayerInRange(range) && currState != BossState.Die && currState != BossState.Paralyze && currState != BossState.Invincible)
            {
                currState = BossState.Idle;
            }
            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange && currState != BossState.Paralyze && currState != BossState.Invincible)
            {
                currState = BossState.Attack;
            }

        }
        else
        {
            currState = BossState.Idle;
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
                case(BossType.Melee):
                    GameController.DamagePlayer(playerDamage);                    
                    
                    AudioSource.PlayClipAtPoint(audioHit, transform.position);

                    StartCoroutine(Cooldown());
                break;
                case(BossType.Ranged):
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
            currState = BossState.Follow;
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

    IEnumerator Paralyze()
    {
        yield return new WaitForSeconds(paralyzedTime);
        currState = BossState.Idle;
    }

    public void ParalyzeEnemy()
    {
        currState = BossState.Paralyze;
    }

    public void DamageEnemy(int damage)
    {
        bossHealth -= damage;
        bossRoom.EnemySpawn();

        if(bossHealth <= 0)
        {
            Death();
        }

        currState = BossState.Invincible;
    }

    void Invincible()
    {
        hasShield = true;
    }

    public void ResetState()
    {
        currState = BossState.Idle;
    }

}
