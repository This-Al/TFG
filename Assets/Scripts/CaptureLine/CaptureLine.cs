using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]

public class CaptureLine : MonoBehaviour
{
    TrailRenderer trailRenderer;
    PolygonCollider2D polygonCollider;
    GameObject player;

    private Vector2 currPos;
    public bool cooldown = false;
    

    Vector3[] trailPointsV3 = new Vector3[100];
    Vector2[] trailPointsV2 = new Vector2[100];

    void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
        polygonCollider = this.GetComponent<PolygonCollider2D>();

        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
        
    }
    
    public void SetColliderPointsFromTrail()
    {
        trailRenderer.GetPositions(trailPointsV3);
        for(int i = 0; i < trailPointsV3.Length; i++)
        {
            Vector2 aux;
            aux = trailPointsV3[i] - transform.position;
            trailPointsV2[i] = aux;
        }
        polygonCollider.SetPath(0, trailPointsV2);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().ParalyzeEnemy();
            player.GetComponent<PlayerController>().DestroyLine();
        }

        if(other.tag == "Boss")
        {
            other.gameObject.GetComponent<BossController>().ParalyzeEnemy();
            player.GetComponent<PlayerController>().DestroyLine();
        }
    }
}
