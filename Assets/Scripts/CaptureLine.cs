using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]

public class CaptureLine : MonoBehaviour
{
    TrailRenderer trailRenderer;
    EdgeCollider2D edgeCollider;

    void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
        edgeCollider = this.GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        SetColliderPointsFromTrail(trailRenderer, edgeCollider);
    }

    public void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();

        for(int position = 0; position < trail.positionCount; position++)
        {
            points.Add(trail.GetPosition(position) - transform.position);
        }
        collider.SetPoints(points);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Circulo cerrado!");
        }
    }    

    void OnDestroy()
    {
        if(edgeCollider != null)
        {
            edgeCollider.enabled = false;
        }
    }
}
