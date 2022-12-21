using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using this class for testing 
public class TestingScript : MonoBehaviour
{
    // bowling ball gameobject 
    [SerializeField] GameObject bowlingBall1;
    [SerializeField] GameObject bowlingBall2;
    [SerializeField] VRJoySticks JoySticks;
    [SerializeField] GameObject pinParent; 
    
    private Rigidbody Rb; 
    private Vector3 ballPosition;
    private Transform[] Balls = new Transform[2];
    private Vector3[] ballLocations = new Vector3[2];
    private Vector3[] pinLocations = new Vector3[10]; 
    private Vector3 zeroVector; 
    private int i; 

    // Start is called before the first frame update
    void Start()
    {
        Balls[0] = bowlingBall1.gameObject.transform;
        Balls[1] = bowlingBall2.gameObject.transform;

        i = 0; 
        foreach (Transform ball in Balls)
        {
            ballLocations[i] = ball.gameObject.transform.position;
            i++; 
        }

        i = 0; 
        foreach(Transform pin in pinParent.transform)
        {
            
            pinLocations[i] = pin.gameObject.transform.position;  
            Debug.Log(pinLocations[i]);
            i++;
        }

        zeroVector = new Vector3(0, 0, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        // if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        if (JoySticks.Right.A > 0.5f)
        {
            // reset bowling ball position
            
            i = 0; 
            foreach(Transform ball in Balls)
            {
                
                Rb = ball.gameObject.GetComponent<Rigidbody>();
                Rb.velocity = zeroVector; 
                Rb.angularVelocity = zeroVector; 
                ball.gameObject.transform.position = ballLocations[i];
                i++; 
            }

            // reset pin positon
            i = 0;
            foreach(Transform pin in pinParent.transform)
            {
                Rb = pin.gameObject.GetComponent<Rigidbody>();
                Rb.velocity = zeroVector; 
                Rb.angularVelocity = zeroVector;
                pin.transform.eulerAngles = zeroVector;
                pin.gameObject.transform.position = pinLocations[i];
                i++; 
            }
        }
    }
}
