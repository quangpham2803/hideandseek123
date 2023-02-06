using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using EnhancedUI.EnhancedScroller;

public class FriendItem2 : EnhancedScrollerCellView
{
    public TMPro.TextMeshProUGUI name;
    public TMPro.TextMeshProUGUI status;
    public TMPro.TextMeshProUGUI cup;
    public string playfabId;
    public void Unfriend()
    {
        for (int i = 0; i < Launcher2.Instance._friends.Count; i++)
        {
            if (Launcher2.Instance._friends[i].FriendPlayFabId == playfabId)
            {                
                Launcher2.Instance.RemoveFriend(Launcher2.Instance._friends[i]);
                Launcher2.Instance.GetPlayerFriendList(playfabId, 1, null);
                Launcher2.Instance.GetFriends();
                break;
            }
        }
    }
    public void GetPlayerProfile(string playFabId)
    {
        
        playfabId = playFabId;
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,
                ShowStatistics = true
            }
        },
        result =>
        {
            name.text = result.PlayerProfile.DisplayName;
            //Get Cup
            foreach (var i in result.PlayerProfile.Statistics)
            {
                if (i.Name == "isOnline")
                {
                    int j = i.Value;
                    if (j == 1)
                    {
                        status.text = "Online";
                        status.color = Color.green;
                    }
                    else
                    {
                        status.text = "Offline";
                        status.color = Color.red;
                    }
                }
                if (i.Name == "cup")
                {
                    cup.text = i.Value.ToString();
                }
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

}
