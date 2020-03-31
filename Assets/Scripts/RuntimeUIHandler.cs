using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RuntimeUIHandler : MonoBehaviour
{
    public void OnClick_MainMenuButton()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    public void OnClick_ExitGameButton()
    {
        Application.Quit();
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();
        //while leaving room
        while (PhotonNetwork.InRoom)
            yield return null;

        //load main menu
        SceneManager.LoadScene(0);  

    }

}
