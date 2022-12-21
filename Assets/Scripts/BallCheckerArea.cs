using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCheckerArea : MonoBehaviour
{
    // Start is called before the first frame update
    public bool ballThrown; //has the ball reached the end of the alleyway?
    [SerializeField] List<GameObject> knockedPins = new List<GameObject>();
    void Start()
    {
        
    }

    public List<GameObject> GetKnockedPins()
    {
        return knockedPins;
    }

    public bool HasBallBeenThrown()
    {
        return ballThrown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("BowlingBall"))
        {
            ballThrown = true;
        }

        if (collider.CompareTag("Pin"))
        {
            knockedPins.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Ball left");
        if (other.CompareTag("BowlingBall")) {
            ballThrown = false;
        }
    }
}
