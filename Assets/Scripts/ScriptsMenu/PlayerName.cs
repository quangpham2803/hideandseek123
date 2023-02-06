using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerName : MonoBehaviourPunCallbacks
{
    public Text playerNameText;

    private void Start()
    {
        PhotonNetwork.NickName = Launcher2.Instance.playerName;
        UpdateName();
    }
    // Update is called once per frame
    void UpdateName()
    {
        playerNameText.text = Launcher2.Instance.playerName;
        PhotonNetwork.NickName = Launcher2.Instance.playerName;
    }
}
