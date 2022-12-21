using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using VelNet;
using System.IO;



[System.Serializable]
public class gamePlayer {
    VelNetPlayer netPlayer;
    public int userID;
    public int bowlScore;
    public int matchScore;
    public int frame;
    public int throwAttempt;

    public gamePlayer(VelNetPlayer netPlayer) {
        this.netPlayer = netPlayer;
        userID = netPlayer.userid;
        bowlScore = 0;
        matchScore = 0;
        frame = 1;
        throwAttempt = 0;
    }

    public VelNetPlayer getVelNetPlayer() {
        return netPlayer;
    }
}


public class BlakeGameManager : NetworkSerializedObjectStream
{
    public GameObject bowlingBall;
    [SerializeField] GameObject pinParent;
    [SerializeField] TextMeshProUGUI scoreBoardP1;
    [SerializeField] TextMeshProUGUI totalScoreP1;
    [SerializeField] TextMeshProUGUI scoreBoardP2;
    [SerializeField] TextMeshProUGUI totalScoreP2;
    [SerializeField] BallCheckerArea ballCheckArea;
    [SerializeField] List<GameObject> livingPins;
    [SerializeField] List<GameObject> knockedPins;
    [SerializeField] List<gamePlayer> players;
    [SerializeField] Animator Spectator1;
    [SerializeField] Animator Spectator2;
    [SerializeField] Animator Spectator3;
    [SerializeField] Animator Spectator4;
    [SerializeField] float knockDownLevel;
    [SerializeField] int totalFrames = 4;
    public int currentPlayer; // 0 - Player 1, 1 - Player 2
    public NetworkPlayer thisPlayer;



    //[SerializeField] List<Vector3> savedPinLocations;
    Vector3 ballLocation = new Vector3();
    Rigidbody rb; 
    Vector3 zeroVector;
    private int pinIndex;
    private string bowlScoreText;
    private string matchScoreText;

    //public enum GAME_STATE{SETUP, WAIT_FOR_THROW}



    //Network Stuff
   protected override void SendState(BinaryWriter binaryWriter)
    {
        foreach (gamePlayer player in players)
        {
            binaryWriter.Write(player.bowlScore);
            binaryWriter.Write(player.matchScore);
            binaryWriter.Write(player.frame);
            binaryWriter.Write(player.throwAttempt);
        }
        binaryWriter.Write(currentPlayer);
    }

    protected override void ReceiveState(BinaryReader binaryReader)
    {
        foreach (gamePlayer player in players)
        {
            player.bowlScore = binaryReader.ReadInt32();
            player.matchScore = binaryReader.ReadInt32();
            player.frame = binaryReader.ReadInt32();
            player.throwAttempt = binaryReader.ReadInt32();

        }
        this.currentPlayer = binaryReader.ReadInt32();
    }

    public void switchTurn()
    {
        VelNetManager.TakeOwnership(bowlingBall.GetComponent<NetworkObject>().networkId);
        VelNetManager.TakeOwnership(this.GetComponent<NetworkObject>().networkId);
    }


    // Start is called before the first frame update
    void Start()
    {
        players = new List<gamePlayer>();
        ballLocation = bowlingBall.transform.position;
        pinIndex = 0;
        currentPlayer = 0;
        foreach(Transform pin in pinParent.transform)
        {
            livingPins.Add(pin.gameObject);
            pin.GetComponent<Pin>().defaultPosition = pin.position;
            pinIndex++;
        }
        
        zeroVector = new Vector3(0, 0, 0);
        bowlScoreText = "";
        matchScoreText = "";
    }

    public void BeginGame() //wait for all players to enter the room, assigned to button in UI
    {
        StartCoroutine(BeginTurn());
        //SendRPC("doBeginGame", true);
    }

    public void doBeginTurn() {
        Debug.Log("STARTING MY TURN");
        StartCoroutine(BeginTurn());
    }

    public void nextPlayer() {
        SendRPC("doBeginTurn", false);
    }

    public void addToSB(string toAdd, int playerNum) {
        MemoryStream mem = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(mem);
        writer.Write(toAdd);
        writer.Write(playerNum);
        SendRPC("doAddToSB", true, mem.ToArray());
    }
    public void setTotalScore(string toSet, int playerNum) {
        MemoryStream mem = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(mem);
        writer.Write(toSet);
        writer.Write(playerNum);
        SendRPC("doSetTotalScore", true, mem.ToArray());
    }


    public void doAddToSB(byte[] data) {
        MemoryStream mem = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(mem);
        string score = reader.ReadString();
        int playerNum = reader.ReadInt32();
        if (playerNum == 0) {
            scoreBoardP1.text += score;
        } else {
            scoreBoardP2.text += score;
        }
    }

    public void doSetTotalScore(byte[] data) {
        MemoryStream mem = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(mem);
        string score = reader.ReadString();
        int playerNum = reader.ReadInt32();
        if (playerNum == 0)
        {
            totalScoreP1.text = score;
        }
        else
        {
            totalScoreP2.text = score;
        }
    }

    public void animate(string trigger) {
        MemoryStream mem = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(mem);
        writer.Write(trigger);
        SendRPC("doAnimate", true, mem.ToArray());
    }
    public void doAnimate(byte[] data) {
        MemoryStream mem = new MemoryStream(data);
        BinaryReader reader = new BinaryReader(mem);
        string trigger = reader.ReadString();
        Debug.Log(trigger);
        Spectator1.SetTrigger(trigger);
        Spectator2.SetTrigger(trigger);
        Spectator3.SetTrigger(trigger);
        Spectator4.SetTrigger(trigger);
    }

    IEnumerator BeginTurn()
    {
        //TAKE OWNERSHIP OF EVERYTHING
        
        Debug.Log("Taking Ownership");
        Debug.Log(bowlingBall.GetComponent<NetworkObject>().networkId);
        Debug.Log(this.GetComponent<NetworkObject>().networkId);
        foreach (Transform pin in pinParent.transform) {
            VelNetManager.TakeOwnership(pin.gameObject.GetComponent<NetworkObject>().networkId);
        }
        VelNetManager.TakeOwnership(bowlingBall.GetComponent<NetworkObject>().networkId);
        VelNetManager.TakeOwnership(this.GetComponent<NetworkObject>().networkId);
        gamePlayer player = players[currentPlayer];
        ResetBallAndPins(player);

        while (player.frame <= totalFrames)
        {
            while (player.throwAttempt < 2)
            {

                while (ballCheckArea.HasBallBeenThrown() == false)
                {
                    yield return new WaitForSeconds(1.0f);

                }
                Debug.Log("add attempt");
                player.throwAttempt++;
                yield return new WaitForSeconds(2.0f); //wait for pins to fall off alley
                TallyScore(player);
                Debug.Log("after tally");
            }
            player.throwAttempt = 0;
            player.frame++;
            Debug.Log("add frame");
        }
        if (currentPlayer == 0)
        {
            currentPlayer = 1;
        }
        else {
            currentPlayer = 0;
        }
        yield return new WaitForSeconds(3.0f);
        SendRPC("doBeginTurn", false);
    }

    void TallyScore(gamePlayer player)
    {
        List<GameObject> tempKnockedPins = new List<GameObject>(ballCheckArea.GetKnockedPins());
        foreach (GameObject pin in livingPins) {
            Quaternion rot = pin.transform.rotation;
            if (pin.transform.rotation.w < knockDownLevel) {
                tempKnockedPins.Add(pin);
            }
            Debug.Log(pin.transform.rotation.w);
        }
        foreach (GameObject knockedPin in tempKnockedPins)
        {
            if (livingPins.Contains(knockedPin)) //if one of the knocked pins is in the living pins list, remove it and add to knocked pins list
            {
                livingPins.Remove(knockedPin);
                knockedPins.Add(knockedPin);
                knockedPin.transform.position += new Vector3(0, -1, 0);
                //knockedPin.SetActive(false);
            }
        }
        if (knockedPins.Count > 1) {
            animate("Cheer");
        }
        player.bowlScore = knockedPins.Count - player.bowlScore; //score for that specific throw
        player.matchScore += player.bowlScore; //add to your match total
        setTotalScore(player.matchScore.ToString(), currentPlayer);
        //totalScore.text = player.matchScore.ToString();

        if (player.throwAttempt < 2)
        {
            addToSB(player.bowlScore + ", ", currentPlayer);
            //scoreBoard.text += player.bowlScore + ", ";
            NextThrow();
        }
        else
        {
            addToSB(player.bowlScore + " | ", currentPlayer);
            //scoreBoard.text += player.bowlScore + " | ";
            ResetBallAndPins(player);
        }
    }
    
    void NextThrow()
    {
        pinIndex = 0;
        foreach (GameObject pin in livingPins) //reset living pins to their original position and rotation
        {
            rb = pin.GetComponent<Rigidbody>();
            rb.velocity = zeroVector; 
            rb.angularVelocity = zeroVector; 
            pin.transform.eulerAngles = zeroVector;
            pin.transform.position = pin.GetComponent<Pin>().defaultPosition;
            pinIndex++;
        }
        ResetBowlingBall();
        ballCheckArea.ballThrown = false;
    }

    void ResetBallAndPins(gamePlayer player)
    {
        foreach (GameObject pin in knockedPins) //set the knocked pins active again
        {
            //pin.SetActive(true);
        }
        livingPins.Clear(); //reset lists and repopulate livingPins again
        knockedPins.Clear();
        ballCheckArea.GetKnockedPins().Clear();
        pinIndex = 0;
        foreach (Transform pin in pinParent.transform)
        {
            livingPins.Add(pin.gameObject);
        }
        foreach (GameObject pin in livingPins) //reset living pins to their original position and rotation
        {
            rb = pin.GetComponent<Rigidbody>();
            rb.velocity = zeroVector; 
            rb.angularVelocity = zeroVector; 
            pin.transform.eulerAngles = zeroVector;
            pin.transform.position = pin.GetComponent<Pin>().defaultPosition;
            pinIndex++;
        }
        ResetBowlingBall();
        ballCheckArea.ballThrown = false;
        
        player.bowlScore = 0;
    }

    public void ResetBowlingBall()
    {
        rb = bowlingBall.GetComponent<Rigidbody>();
        rb.velocity = zeroVector;
        rb.angularVelocity = zeroVector; 
        bowlingBall.transform.position = ballLocation;
        bowlingBall.transform.eulerAngles = zeroVector;
    }
    

    // Update is called once per frame
    void Update()
    {
        
        foreach (VelNetPlayer p in VelNetManager.Players)
        {
            if (players.Find(player => player.getVelNetPlayer() == p) == null) {
                players.Add(new gamePlayer(p));
            }
        }
        
        //Debug.Log(ballCheckArea.GetComponent<BallCheckerArea>().HasBallBeenThrown()); //checks if ball has been thrown
        //Debug.Log(ballCheckArea.GetKnockedPins().Count); //number of pins off alley
    }


}
