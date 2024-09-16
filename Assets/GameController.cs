using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int loadedPlayers = 0;

    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void OnLoad()
    {
        loadedPlayers += 1;

        if (loadedPlayers == PhotonNetwork.CountOfPlayers)
        {
            Player[] Players = PhotonNetwork.PlayerList;
            GameObject[] PlayerCharacters = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < Players.Count(); i++)
            {

            }
        }
    }
}
