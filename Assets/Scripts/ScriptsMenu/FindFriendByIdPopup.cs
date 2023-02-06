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

public class FindFriendByIdPopup : MonoBehaviour
{
    public TMP_InputField CodeInputField;
    enum FriendIdType { PlayFabId, Username, Email, DisplayName };

    void AddFriendPlayFab(FriendIdType idType, string friendId)
    {
        var request = new PlayFab.ServerModels.AddFriendRequest();
        request.PlayFabId = Launcher2.Instance.MyPlayfabID;
        request.FriendPlayFabId = friendId;

        PlayFabServerAPI.AddFriend(request, result => {
            Launcher2.Instance.AddFriendPlayFab(CodeInputField.text, Launcher2.Instance.MyPlayfabID);
            Launcher2.Instance.GetFriends();
            StartCoroutine(Launcher2.Instance.waitToDone(CodeInputField.text));
            Launcher2.Instance.AlertPopup("REQUEST SUCCESSFULLY");           
            Debug.Log("Friend added successfully!");
            gameObject.SetActive(false);
        }, DisplayPlayFabError);
    }

    void DisplayPlayFabError(PlayFabError error) { Launcher2.Instance.AlertPopup(error.ErrorMessage);/*Debug.Log(error.GenerateErrorReport());*/ }
    public void SentRequest()
    {
        AddFriendPlayFab(FriendIdType.PlayFabId, CodeInputField.text);
        
    }
}
