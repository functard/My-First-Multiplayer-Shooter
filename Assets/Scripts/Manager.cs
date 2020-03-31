using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Manager : MonoBehaviour
{
    [SerializeField]
    Transform[] spawnPoints;

    [SerializeField]
    private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[PhotonNetwork.CountOfPlayersInRooms % 2].transform.position, playerPrefab.transform.rotation);
    }
}
