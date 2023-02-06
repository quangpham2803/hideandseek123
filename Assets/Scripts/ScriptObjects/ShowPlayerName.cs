using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class ShowPlayerName : MonoBehaviourPun
{
    public bool isBot;
    public TMPro.TextMeshProUGUI playerName;
    void Start()
    {
        if (isBot == false)
        {
            playerName.text = photonView.Owner.NickName;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RandomName", RpcTarget.AllBuffered);
            }
        }
    }
    private void LateUpdate()
    {
        if (GameManager.instance.mainPlayer.cam !=null)
        {
            transform.LookAt(transform.position + GameManager.instance.mainPlayer.cam.transform.forward);
        }
    }
    //private string[] names = new string[] { "Peter", "Ron", "Satchmo","Ngoc","Khai","Mr K","Mr N","Bo Bo" };
    //public string GetRandomName()
    //{
    //    return names[Random.Range(0, names.Length)];
    //}
    [PunRPC]
    void RandomName()
    {
        playerName.text = "guest" + Random.Range(0, 999999).ToString("000000");
    }
}
