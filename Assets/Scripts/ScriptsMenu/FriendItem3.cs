using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using EnhancedUI.EnhancedScroller;

public class FriendItem3 : EnhancedScrollerCellView
{
    public TMPro.TextMeshProUGUI name;
    public TMPro.TextMeshProUGUI status;
    public GameObject buttonInvite;
    public string playfabId;
    bool canInvite;
    public void GetPlayerProfile(string playFabId)
    {
        playfabId = playFabId;
        PlayFabClientAPI.GetPlayerProfile(new PlayFab.ClientModels.GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayFab.ClientModels.PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,
                ShowStatistics = true
            }
        },
        result =>
        {
            name.text = result.PlayerProfile.DisplayName;
            int tm = 0;
            int j = 0;
            //Get Cup
            foreach (var i in result.PlayerProfile.Statistics)
            {               
                if(i.Name == "status")
                {
                    tm = i.Value;
                }
                if (i.Name == "isOnline")
                {
                    j = i.Value;
                }
            }
            if (j == 1)
            {
                if (tm == 0)
                {
                    status.text = "Lobby";
                    status.color = Color.green;
                    buttonInvite.SetActive(true);
                    canInvite = true;
                }
                else
                {
                    status.text = "Playing";
                    status.color = Color.red;
                    buttonInvite.SetActive(false);
                    canInvite = false;
                }
            }
            else
            {
                status.text = "Offline";
                status.color = Color.red;
                canInvite = false;
                buttonInvite.SetActive(false);
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }
    public void InviteClick()
    {
        Launcher2.Instance.GetPlayerFriendList(playfabId, 3, Photon.Pun.PhotonNetwork.CurrentRoom.Name);
        buttonInvite.SetActive(false);
    }
}
