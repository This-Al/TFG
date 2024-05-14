using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CapturePoint : MonoBehaviour
{
    GameObject captureLine;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        captureLine = GameObject.FindGameObjectWithTag("CaptureLine");

        StartCoroutine(InitiateCollider());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            SuccessfulCapture();
        }
    }

    void SuccessfulCapture()
    {
        Debug.Log("Capture");
        captureLine.GetComponent<CaptureLine2>().SetColliderPointsFromTrail();

        //player.GetComponent<PlayerController>().DestroyLine();
    }

    IEnumerator InitiateCollider()
    {
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<CircleCollider2D>().enabled = true;
    }
}
