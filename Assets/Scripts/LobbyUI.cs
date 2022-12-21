using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] GameObject GameRoomPrefab;
    [SerializeField] Transform StartRoomAnchor;
    [SerializeField] float AnchorShift;
    [SerializeField] GameManager gameManager;

    [SerializeField] Text CurrentRoomInput;
    [SerializeField] Transform OpenRoomsBoard;
    [SerializeField] GameObject OpenRoomButtonPrefab;
    // each open room is associated with a button in the lobby
    // and a transform to teleport to
    List<Button> Buttons;

    [SerializeField] bool EditorRefreshRooms;
    // Start is called before the first frame update
    void Start()
    {
        Buttons = new List<Button>();

        RefreshRooms();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EditorRefreshRooms)
        {
            EditorRefreshRooms = false;
            RefreshRooms();
        }
    }

    public void JoinRoom()
    {

        if (CurrentRoomInput.text == "") return;

        gameManager.joinRoom(CurrentRoomInput.text);
        // teleport player to place grabbed

    }

    public void RefreshRooms()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            var button = Buttons[i];
            Buttons.Remove(button);
            Destroy(button.gameObject);
        }

        foreach (string room in gameManager.RoomList)
        {
            GameObject newRoomButton = Instantiate(OpenRoomButtonPrefab, OpenRoomsBoard);
            var button = newRoomButton.GetComponent<Button>();
            var text = newRoomButton.GetComponentInChildren<Text>();
            text.text = room;
            button.onClick.AddListener(() => { CurrentRoomInput.text = text.text; });
            Buttons.Add(button);
        }
    }


    public void AddText(Text text)
    {
        CurrentRoomInput.text += text.text;
    }

    public void BackSpace()
    {
        var textLength = CurrentRoomInput.text.Length;
        if (textLength > 0)
        {
            CurrentRoomInput.text = CurrentRoomInput.text.Remove(textLength - 1); // remove last character
        }
    }
}
