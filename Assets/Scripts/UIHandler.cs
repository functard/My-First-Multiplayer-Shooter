using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class UIHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField createRoomInputField;

    [SerializeField]
    private InputField joinRoomInputField;


    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInputField.text,null);
    }


    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomInputField.text,new RoomOptions { MaxPlayers = 2 },null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room failed, " + returnCode + " Message, " + message);
    }

    
}
