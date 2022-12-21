using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using VelNet;
using System.Linq.Expressions;

public class GameManager : MonoBehaviour
{

    string ReadyPlayerMeURL;
    [SerializeField] string EditorReadyPlayerMeURL;
    [SerializeField] Player currentPlayer; // Reference to this application's player
    private NetworkObject netPlayer;
    public List<string> RoomList;
    [SerializeField] GameObject XROrigin;
    [SerializeField] Transform RinkTeleportAnchor;
    [SerializeField] Transform AvatarTeleportAnchor;
    [SerializeField] BlakeGameManager blakeGameManager;
    // Start is called before the first frame update
    private void Awake()
    {
        RoomList = new List<string>();
    }
    void Start()
    {

        // Read url from file
        string FilePath = Path.Combine(Application.dataPath, "ReadyPlayerMeURL.txt");
        try
        {
            using (var reader = new StreamReader(FilePath))

            {
                ReadyPlayerMeURL = reader.ReadLine();
            }
        }
        catch (IOException)
        {
            ReadyPlayerMeURL = null;
        }


        if (ReadyPlayerMeURL == null)
        {
            ReadyPlayerMeURL = EditorReadyPlayerMeURL;
        }

        VelNetManager.OnLoggedIn += () =>
        {
            // Temp join to make sure it's all sinked, will be done from UI later
            //joinRoom("2");
            updateRoomList();
        };

        VelNetManager.OnJoinedRoom += (roomName) =>
        {
            Debug.Log("Joined Room " + roomName);
            updateRoomList();
            XROrigin.transform.position = AvatarTeleportAnchor.position;
        };

        VelNetManager.OnLeftRoom += (roomName) =>
        {
            netPlayer.GetComponent<NetworkPlayer>().destroyAvatar();
            VelNetManager.NetworkDestroy(netPlayer);
        };
    }

    public void selectAvatar(string avatarURL) {
        NetworkObject player = VelNetManager.NetworkInstantiate("Player");
        netPlayer = player;
        player.GetComponent<NetworkPlayer>().player = currentPlayer;
        player.GetComponent<NetworkPlayer>().blakeGameManager = this.blakeGameManager;
        player.GetComponent<NetworkPlayer>().loadAvatar(avatarURL);
        // Teleport player to room
        XROrigin.transform.position = RinkTeleportAnchor.position;
        //Delete the player's controllers here
        Transform rayInteractor = XROrigin.transform.FindChildRecursive("Ray Interactor");
        if (rayInteractor != null)
        {
            //Destroy(rayInteractor.gameObject);
        }
        Transform XRControllerLeft = XROrigin.transform.FindChildRecursive("XRControllerLeft");
        if (XRControllerLeft != null)
        {
            foreach (MeshRenderer controllerPart in XRControllerLeft.GetComponentsInChildren<MeshRenderer>())
            {
                controllerPart.enabled = false;
            }
        }
        Transform XRControllerRight = XROrigin.transform.FindChildRecursive("XRControllerRight");
        if (XRControllerRight != null)
        {
            foreach (MeshRenderer controllerPart in XRControllerRight.GetComponentsInChildren<MeshRenderer>())
            {
                controllerPart.enabled = false;
            }
        }
    }
    public void joinRoom(string roomName)
    {
        VelNetManager.Join(roomName);
        updateRoomList();
    }

    public void updateRoomList()
    {

        RoomList.Clear();

        VelNetManager.GetRooms((data) =>
        {
            data.rooms.ForEach((listedRoom) =>
            {
                RoomList.Add(listedRoom.name);
            });
        });
    }


    // Update is called once per frame
    void Update()
    {

    }
}
