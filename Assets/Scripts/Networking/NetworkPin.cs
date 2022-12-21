using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VelNet;

public class NetworkPin : NetworkSerializedObjectStream
{
    protected override void ReceiveState(BinaryReader binaryReader)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();

        this.transform.position = binaryReader.ReadVector3();
        this.transform.rotation = binaryReader.ReadQuaternion();

        rb.velocity = binaryReader.ReadVector3();
        rb.angularVelocity = binaryReader.ReadVector3();
        
    }

    protected override void SendState(BinaryWriter binaryWriter)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();

        binaryWriter.Write(transform.position);
        binaryWriter.Write(transform.rotation);

        binaryWriter.Write(rb.velocity);
        binaryWriter.Write(rb.angularVelocity);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
