using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBallBehaviour1 : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip rollingSoundClip;
    [SerializeField] private AudioClip pinsHitSoundClip;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with ball");
        //AudioSource.PlayClipAtPoint(pinsHitSoundClip, transform.position);

        if (collision.gameObject.CompareTag("BowlingLane"))
        {
            Debug.Log("Playing bowling lane");
            AudioSource.PlayClipAtPoint(rollingSoundClip, transform.position);
        }

        if (collision.gameObject.CompareTag("Pin"))
        {
            Debug.Log("Hitting Pins");
            audioSource.Stop();
            AudioSource.PlayClipAtPoint(pinsHitSoundClip, transform.position);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
