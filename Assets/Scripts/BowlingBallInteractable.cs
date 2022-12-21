using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VelNet;

public class BowlingBallInteractable : XRGrabInteractable

{

    [SerializeField] float ThrowVelocityFactor = 2f;
    bool check = false;
    Vector3 velocity;
    Vector3 angularVelocity;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().maxAngularVelocity = float.PositiveInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called every frame while object is selected
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        //Don't grab ball if not your network object
        if (!networkObject.IsMine) return;
        base.ProcessInteractable(updatePhase);
        if (interactorsSelecting.Count > 0)
        {
            // for ownership
            Transform HandHolding = interactorsSelecting[0].transform;
        }
    }

    // Called on frame when object is thrown
    protected override void OnSelectExiting(SelectExitEventArgs args)
    { 
        base.OnSelectExiting(args);
        // for updating ball at throw
        velocity = gameObject.GetComponent<Rigidbody>().velocity;
        angularVelocity = gameObject.GetComponent<Rigidbody>().angularVelocity;
        gameObject.GetComponent<NetworkBall>().BallLetGo();
        check = true;
    }

    void LateUpdate()
    {
        if (check)
        {
            // for ownership
            gameObject.GetComponent<Rigidbody>().velocity = velocity * ThrowVelocityFactor;
            gameObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity * ThrowVelocityFactor;
            check = false;
        }

    }

}
