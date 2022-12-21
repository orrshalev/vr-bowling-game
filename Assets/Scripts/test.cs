using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    OVRInput.Controller LeftController;
    OVRInput.Controller RightController;
    void Start()
    {
        LeftController = OVRInput.Controller.LTouch;
        RightController = OVRInput.Controller.RTouch;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(string.Format("\nLeft Controller Velocity: {0}\nRight Controller Velocity: {1}", OVRInput.GetLocalControllerVelocity(LeftController), OVRInput.GetLocalControllerVelocity(RightController)));
    }
}
