using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject connectedScreen;

    [SerializeField]
    private GameObject connectionFailScreen;

    public void OnClick_BattleButton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {     
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionFailScreen.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        if (connectionFailScreen.activeSelf)
            connectionFailScreen.SetActive(false);


        connectedScreen.SetActive(true);
    }

    
}
