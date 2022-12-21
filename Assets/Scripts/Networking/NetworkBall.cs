using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VelNet;
public class NetworkBall : NetworkSerializedObjectStream
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

    public void BallLetGo()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        MemoryStream mem = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(mem);
        writer.Write(transform.position);
        writer.Write(rb.velocity);
        writer.Write(rb.angularVelocity);
        SendRPC("doBallLetGo", true, mem.ToArray());
    }

    public void doBallLetGo(byte[] data) {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        MemoryStream mem = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(mem);
        Vector3 pos = reader.ReadVector3();
        Vector3 vel = reader.ReadVector3();
        Vector3 angVel = reader.ReadVector3();
        Debug.Log(vel);

        this.transform.position = pos;
        rb.velocity = vel;
        rb.angularVelocity = angVel;


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
