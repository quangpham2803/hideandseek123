
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    private string _playFabPlayerIdCache;
    public GameObject ironSourceManager;
    public void Awake()
    {
        CreatePlayerAndUpdateDisplayName();        
    }
    void CreatePlayerAndUpdateDisplayName()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        }, result => {
            Debug.Log("Successfully logged in a player with PlayFabId: " + result.PlayFabId);
            OnLoginSuccess(result);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Instantiate(ironSourceManager);
        Launcher2.Instance.GetAccountInfo();
        Launcher2.Instance.GetFriends();
        Launcher2.Instance.isLoggin = true;
        StatusPlayer.Instance.isLogging = true;
        Debug.Log("Login success!!");
        RequestPhotonToken(result);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    private void RequestPhotonToken(LoginResult obj)
    {
        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = "c24b09a4-e3f7-4dd5-be51-a2af8aa49f96"
        }, Success, OnLoginFailure);
    }
    void Success(GetPhotonAuthenticationTokenResult obj)
    {
        Debug.Log("Done");
    }
}