using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health = 6;
    private static int maxHealth = 6;
    private static float moveSpeed = 7f;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;
    private static float trailCharge = 6;
    private static float trailMaxCharge = 6;
    private static float trailCooldown = 3;

    public static bool isLineOnCooldown = false;

    public static float Health{ get => health; set => Health = value; }
    public static int MaxHealth{ get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed{ get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate{ get => fireRate; set => fireRate = value; }
    public static float BulletSize{ get => bulletSize; set => bulletSize = value; }
    public static float TrailCharge{ get => trailCharge; set => trailCharge = value; }
    public static float TrailMaxCharge{ get => trailMaxCharge; set => trailMaxCharge = value; }

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

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if(Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void DrainCharge(float charge)
    {
        trailCharge -= charge;

        if(trailCharge <= 0)
        {
            ChargeCooldown();
        }
    }

    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, Health + healAmount);
    }

    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void FireRateChange(float rate)
    {
        fireRate -= rate;
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }

    private static void KillPlayer()
    {

    }

    public static void TrailChargeChange(float chargeAmount)
    {
        trailMaxCharge += chargeAmount;
    }

    public static IEnumerator ChargeCooldown()
    {
        trailCharge = 0;
        isLineOnCooldown = true;
        yield return new WaitForSeconds(trailCooldown);
        isLineOnCooldown = false;
        trailCharge = trailMaxCharge;
    }
}
