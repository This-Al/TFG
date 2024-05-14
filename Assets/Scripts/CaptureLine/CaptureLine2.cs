using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]

public class CaptureLine2 : MonoBehaviour
{
    TrailRenderer trailRenderer;
    PolygonCollider2D polygonCollider;
     GameObject player;

    private Vector2 currPos;
    private int trailIndex = 0;
    //List<Vector3> trailPoints = new List<Vector3>();
    Vector3[] trailPointsV3 = new Vector3[200];
    Vector2[] trailPointsV2 = new Vector2[200];

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
            aux = trailPointsV3[i] + transform.parent.position;
            Debug.Log(aux.ToString());
            trailPointsV2[i] = aux;
        }
        polygonCollider.SetPath(0, trailPointsV2);
        Debug.Log("Collider colocado");
    }

    void OnDestroy()
    {
        if(polygonCollider != null)
        {
            polygonCollider.enabled = false;
        }
    }
}
