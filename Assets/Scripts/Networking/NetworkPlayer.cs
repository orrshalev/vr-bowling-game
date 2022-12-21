using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VelNet;
using ReadyPlayerMe;
public class NetworkPlayer : NetworkSerializedObjectStream
{

    public Transform headTransform, LHandTransform, RHandTransform;
    public Player player;
    public GameObject avatar;
    public string avatarUrl;
    public bool switchTurns;
    public BlakeGameManager blakeGameManager;
    public void loadAvatar(string url) 
    {
        MemoryStream mem = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(mem);
        writer.Write(url);
        SendRPC("doLoadAvatar", true, mem.ToArray());    
    }

    public void doLoadAvatar(byte[] data)
    {
        MemoryStream mem = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(mem);
        string url = reader.ReadString();
        avatarUrl = url;
        Debug.Log(url);
        if (blakeGameManager)
        {
           // blakeGameManager.players.Add(url);
        }
        var avatarLoader = new AvatarLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            avatar = args.Avatar;
            LHandTransform = avatar.transform.FindChildRecursive("LeftHand");
            RHandTransform = avatar.transform.FindChildRecursive("RightHand");
            headTransform = avatar.transform.FindChildRecursive("Hips");

        };
        avatarLoader.LoadAvatar(url);
    }

    public void doDestroyAvatar() {
        Destroy(avatar);
    }
    public void destroyAvatar() {
        SendRPC("doDestroyAvatar", true);
    }

 /*   public void switchTurn() {
        SendRPC("doSwitchTurn", false);
    }

    public void doSwitchTurn() {
        Debug.Log("STARTING SWITCH");
        Debug.Log(blakeGameManager.bowlingBall.GetComponent<NetworkObject>().networkId);
        string ballID = blakeGameManager.bowlingBall.GetComponent<NetworkObject>().networkId;
        string gameManagerId = blakeGameManager.GetComponent<NetworkObject>().networkId;
        Debug.Log(ballID);
        Debug.Log(gameManagerId);
        
        VelNetManager.TakeOwnership(ballID);
        VelNetManager.TakeOwnership(gameManagerId);
        Debug.Log("SWITCHED TURNS");

    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Do nothing if there is no player
        if (player == null || avatar == null) return;

        if (!networkObject.IsMine) {
            return;
        }

        //Debug.Log(blakeGameManager.bowlingBall.GetComponent<NetworkObject>().networkId);
        if (networkObject.IsMine)
        {
            headTransform.position = player.head.position;
            headTransform.rotation = player.head.rotation;

            LHandTransform.position = player.LHand.position;
            LHandTransform.rotation = player.LHand.rotation;

            RHandTransform.position = player.RHand.position;
            RHandTransform.rotation = player.RHand.rotation;

            transform.position = player.transform.position;
            transform.rotation = player.transform.rotation;
        }
    }

    protected override void SendState(BinaryWriter binaryWriter)
    { 
        binaryWriter.Write(avatarUrl);
        binaryWriter.Write(headTransform.position);
        binaryWriter.Write(headTransform.rotation);
        binaryWriter.Write(LHandTransform.position);
        binaryWriter.Write(LHandTransform.rotation);
        binaryWriter.Write(RHandTransform.position);
        binaryWriter.Write(RHandTransform.rotation);
        binaryWriter.Write(transform.position);
        binaryWriter.Write(transform.rotation);
    }

    protected override void ReceiveState(BinaryReader binaryReader)
    {
        string inURL = binaryReader.ReadString();
            GameObject gameManager = GameObject.Find("BlakeGameManager");
            blakeGameManager = gameManager.GetComponent<BlakeGameManager>();
    
        if (inURL != avatarUrl)
        {
            loadAvatar(inURL);
        }
        headTransform.position = binaryReader.ReadVector3();
        headTransform.rotation = binaryReader.ReadQuaternion();

        LHandTransform.position = binaryReader.ReadVector3();
        LHandTransform.rotation = binaryReader.ReadQuaternion();

        RHandTransform.position = binaryReader.ReadVector3();
        RHandTransform.rotation = binaryReader.ReadQuaternion();

        transform.position = binaryReader.ReadVector3();
        transform.rotation = binaryReader.ReadQuaternion();
    }
}
