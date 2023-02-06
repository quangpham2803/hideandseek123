using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviourPunCallbacks
{
    public GameManager gameManager;
    public GameObject message;
    public GameObject messageItem;
    public GameObject messageHelp;
    public GameObject messageLike;
    public GameObject messageNoHelp;
    private bool messageClick = false;
    public int id;
    private void Update()
    {
    }
    public void ButtonMessageClick()
    {
        Debug.Log("MessageClick");
        if (!messageClick)
        {
            message.GetComponent<Animator>().SetBool("Click", true);
            messageClick = true;
        }
        else
        {
            message.GetComponent<Animator>().SetBool("Click", false);
            messageClick = false;
        }
    }
    
    public void ButtonMessageHelpClick()
    {
        Debug.Log("MessageHelpClick");
        photonView.RPC("ButtonMessageHelpClickPun", RpcTarget.AllBuffered, id);
    }
    public void ButtonMessageLikeClick()
    {
        Debug.Log("MessageLikeClick");
        photonView.RPC("ButtonMessageLikeClickPun", RpcTarget.AllBuffered, id);
    }
    public void ButtonMessageNoHelpClick()
    {
        Debug.Log("MessageNoHelpClick");
        photonView.RPC("ButtonMessageNoHelpClickPun", RpcTarget.AllBuffered, id);
    }

    void ClearList(PlayerSetup _playerSetup)
    {
        foreach (Image item in _playerSetup.messageItem)
        {
            item.gameObject.SetActive(false);
        }
    }
    [PunRPC]
    void ButtonMessageHelpClickPun(int _id)
    {
        PlayerSetup _playerSetup = PhotonView.Find(_id).GetComponent<PlayerSetup>();
        if (GameManager.instance.mainPlayer.player == _playerSetup.player)
        {
            ClearList(_playerSetup);
            _playerSetup.messageHelp.gameObject.SetActive(true);
        }
    }
    [PunRPC]
    void ButtonMessageLikeClickPun(int _id)
    {
        PlayerSetup _playerSetup = PhotonView.Find(_id).GetComponent<PlayerSetup>();
        ClearList(_playerSetup);
        _playerSetup.messageLike.gameObject.SetActive(true);
    }
    [PunRPC]
    void ButtonMessageNoHelpClickPun(int _id)
    {
        PlayerSetup _playerSetup = PhotonView.Find(_id).GetComponent<PlayerSetup>();
        ClearList(_playerSetup);
        _playerSetup.messageNoHelp.gameObject.SetActive(true);
    }
}
