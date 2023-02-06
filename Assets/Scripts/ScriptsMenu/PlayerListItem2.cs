using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
public class PlayerListItem2 : MonoBehaviourPunCallbacks
{
	[SerializeField] TMPro.TextMeshProUGUI text;
    public Photon.Realtime.Player player;
    
    public GameObject buttonKick;
    public GameObject buttonAddFriend;
    public string idPlayer;
    public GameObject checkReady;
    public bool isReady;
    public GameObject kingImg;
    bool isSetup;
    public void SetUp(Photon.Realtime.Player _player, bool _isReady)
	{
		player = _player;
		text.text = _player.NickName;
        isReady = _isReady;
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}
    public void KickClick()
    {
        PhotonNetwork.CloseConnection(player);
        PhotonNetwork.CloseConnection(player);
    }
    public void AddFriend()
    {
        buttonAddFriend.SetActive(false);
        AddFriendPlayFab(FriendIdType.PlayFabId, idPlayer);
        Launcher2.Instance.AddFriendPlayFab(idPlayer, Launcher2.Instance.MyPlayfabID);
        Launcher2.Instance.GetFriends();
        StartCoroutine(Launcher2.Instance.waitToDone(idPlayer));       
    }

    enum FriendIdType { PlayFabId, Username, Email, DisplayName };

    void AddFriendPlayFab(FriendIdType idType, string friendId)
    {
        var request = new PlayFab.ServerModels.AddFriendRequest();
        request.PlayFabId = Launcher2.Instance.MyPlayfabID;
        request.FriendPlayFabId = friendId;
        
        PlayFabServerAPI.AddFriend(request, result => {         
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }

    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
    void DisplayError(string error) { Debug.LogError(error); }
    private void Start()
    {
        isSetup = false;
        if (PhotonNetwork.IsMasterClient)
        {
            Launcher2.Instance.SentNumberPlayerReady();
        }
        StartCoroutine(SetupPlayerListItem());

    }
    
    IEnumerator SetupPlayerListItem()
    {
        Launcher2.Instance.GetDataForPlayListItem();
        yield return new WaitUntil(isHaveId);
        if (idPlayer != Launcher2.Instance.MyPlayfabID)
        {
            buttonAddFriend.SetActive(true);
            for (int i = 0; i < Launcher2.Instance._friends.Count; i++)
            {
                if (Launcher2.Instance._friends[i].FriendPlayFabId == idPlayer)
                {
                    buttonAddFriend.SetActive(false);
                    break;
                }
            }
        }
        else
        {
            buttonAddFriend.SetActive(false);
        }
        isSetup = true;
    }
    IEnumerator SetupPlayerListItemAgain()
    {
        Launcher2.Instance.GetDataForPlayListItemAgain();
        yield return new WaitUntil(isHaveId);
        if (idPlayer != Launcher2.Instance.MyPlayfabID)
        {
            buttonAddFriend.SetActive(true);
            for (int i = 0; i < Launcher2.Instance._friends.Count; i++)
            {
                if (Launcher2.Instance._friends[i].FriendPlayFabId == idPlayer)
                {
                    buttonAddFriend.SetActive(false);
                    break;
                }
            }
        }
        else
        {
            buttonAddFriend.SetActive(false);
        }
    }
    bool isHaveId()
    {
        return !String.IsNullOrEmpty(idPlayer);
    }
    public void Update()
    {
        if (player.IsMasterClient)
        {
            kingImg.SetActive(true);
        }
        else
        {
            kingImg.SetActive(false);
        }
        if (PhotonNetwork.IsMasterClient && !player.IsLocal)
        {
            buttonKick.SetActive(true);
        }
        else
        {
            buttonKick.SetActive(false);            
        }
        checkReady.SetActive(isReady);
        if (String.IsNullOrEmpty(idPlayer))
        {
            StartCoroutine(SetupPlayerListItemAgain());
        }
    }
}