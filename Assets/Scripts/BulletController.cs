using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;
    private Vector2 lastPos;
    private Vector2 currPos;
    private Vector2 playerPos;

    public AudioClip audioHit;
    public AudioClip audioHitBoss;

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade());
        if(!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        } else 
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 60, 60);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(isEnemyBullet)
        {
            currPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if(currPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = currPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        int damage = 1;

        if(col.tag == "Enemy" && !isEnemyBullet && !col.gameObject.GetComponent<EnemyController>().hasShield)
        {
            AudioSource.PlayClipAtPoint(audioHit, transform.position);
            col.gameObject.GetComponent<EnemyController>().DamageEnemy(damage);
            Destroy(gameObject);
        }

        if(col.tag == "Boss" && !isEnemyBullet && !col.gameObject.GetComponent<BossController>().hasShield)
        {
            AudioSource.PlayClipAtPoint(audioHitBoss, transform.position);
            col.gameObject.GetComponent<BossController>().DamageEnemy(damage);
            Destroy(gameObject);
        }

        if(col.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
        
    }
}
