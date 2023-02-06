using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using UnityEngine.SceneManagement;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using EnhancedScrollerDemos.SuperSimpleDemo;

public class Launcher2 : MonoBehaviourPunCallbacks
{
    public static Launcher2 Instance;
    public FadeScene fadeScene;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    [SerializeField] TMP_InputField NameInputField;
    [SerializeField] GameObject changeName;
    public GameObject buttonStart;
    public bool isStart;
    bool isCreate;
    public GameObject button2;
    public GameObject popupBuyEnergy;
    public int numberPlayerReady;
    public bool isReady;

    public string playerName;
    public TMPro.TextMeshProUGUI playerNameText;
    public bool test;
    public bool testWithBot;
    public GameObject currentOpenedRoom;
    void Awake()
    {
        Instance = this;
        isLoggin = false;
        MergeRoomMenu.SetActive(false);
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
        }
        OnConnectedToMaster();
        isStart = false;
        isCreate = false;
        numberPlayerReady = 0;
        currentPlayerMacthing = 0;
        isReady = true;
        isYou = false;
    }
    public TMPro.TextMeshProUGUI numberPlayer;
    public bool isLoggin;
    int currentPlayerMacthing;
    int randomNumber;
    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            numberPlayer.text = PhotonNetwork.PlayerList.Length.ToString();
            if (PhotonNetwork.IsMasterClient && isStart == false)
            {
                buttonStart.SetActive(true);
            }
            else
            {
                buttonStart.SetActive(false);
            }
            if (MergeRoomMenu.activeSelf)
            {
                if (currentPlayerMacthing < PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    StartCoroutine(randomNumberPlayer());
                    currentPlayerMacthing = PhotonNetwork.CurrentRoom.PlayerCount;
                    mergeRoomText.text = currentPlayerMacthing.ToString();
                }
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    mergeRoomText.text = "10";
                    isStart = true;
                    photonView.RPC("AddWhenLoadLevel", RpcTarget.AllBuffered);
                    photonView.RPC("NeedBot", RpcTarget.AllBuffered, true);
                    fadeScene.PhotonLevelLoad("mapgenerate");
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                }
            }
        }
    }
    IEnumerator randomNumberPlayer()
    {
        yield return new WaitForSeconds(Random.RandomRange(1f, 5f));
        randomNumber = Random.RandomRange(1, 3);
        currentPlayerMacthing += randomNumber;
        if (currentPlayerMacthing < 9)
        {
            mergeRoomText.text = currentPlayerMacthing.ToString();
        }
        else
        {
            mergeRoomText.text = "9";
        }
        if (currentPlayerMacthing < 9)
        {
            StartCoroutine(randomNumberPlayer());
        }
    }
    [PunRPC]
    void NeedBot(bool q)
    {
        RoomManager2.Instance.needBot = q;
        RoomManager2.Instance.numberPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        //OnJoinedLobby();
    }
    public Transform playerList;
    IEnumerator WaitToJoinLobby()
    {
        yield return new WaitForSeconds(1f);
    }
    public override void OnJoinedLobby()
    {
        currentOpenedRoom = title;
        isReady = true;
        isMerge = false;
        numberPlayerReady = 0;
        if (isLoggin == true)
        {
            StartCoroutine(GetFriendNotice());
            GetAccountInfo();
            GetFriends();
            RequestLeaderboard();
            RequestLeaderboardForYou();
            StatusPlayer.Instance.ServerGetTitleData();
            StatusPlayer.Instance.StartCoroutine(StatusPlayer.Instance.waitUntilHaveData());            
        }
        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(WaitToJoinLobby());
        LoadingMenu.SetActive(false);
        title.SetActive(true);
        Debug.Log("Joined Lobby");
    }
    public IEnumerator GetFriendNotice()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("update frient");
        GetFriends();
        StartCoroutine(GetFriendNotice());
    }
    public FriendListView friendListView;
    public RequestListView requestListView;
    public SmallList<string> _friendList = null;
    public SmallList<string> _requestList = null;
    public FriendInvite invitePopup;
    public Transform inviteContain;
    public FriendListView1 friendListView1;
    public GameObject requestIcon;
    void DisplayFriends(List<PlayFab.ServerModels.FriendInfo> friendsCache)
    {
        _friendList = new SmallList<string>();
        _requestList = new SmallList<string>();
        //friendsCache.ForEach(f => Debug.Log(f.FriendPlayFabId));
        for (int i = 0; i < friendsCache.Count; i++)
        {
            if (friendsCache[i].Tags != null && friendsCache[i].Tags[0] == "a_friend")
            {
                _friendList.Add(friendsCache[i].FriendPlayFabId);
                if (friendsCache[i].Tags.Count > 1 && friendsCache[i].Tags[1] == "b_invite")
                {
                    string roomPass = friendsCache[i].Tags[2];
                    string friendRegion = friendsCache[i].Tags[3];
                    List<string> tm = new List<string>();
                    tm.Add("a_friend");
                    SetPlayerFriendTags(MyPlayfabID, friendsCache[i], tm);
                    FriendInvite popup = Instantiate(invitePopup, inviteContain);
                    popup.SetUpMess(friendsCache[i].TitleDisplayName, roomPass, friendRegion);
                    GetPlayerFriendList(friendsCache[i].FriendPlayFabId, 0, null);
                }
            }
            else if (friendsCache[i].Tags == null || friendsCache[i].Tags[0] == null)
            {
                _requestList.Add(friendsCache[i].FriendPlayFabId);
                requestIcon.SetActive(true);
            }
            else if (friendsCache[i].Tags != null && friendsCache[i].Tags[0] == "request")
            {
                GetPlayerFriendList(friendsCache[i].FriendPlayFabId, 2, null);
                //_friendList.Add(friendsCache[i].FriendPlayFabId);
            }
        }
        friendListView._data = _friendList;
        friendListView.LoadLargeData();
        requestListView._data = _requestList;
        requestListView.LoadLargeData();
        friendListView1._data = _friendList;
        friendListView1.LoadLargeData();
    }
    void DisplayPlayFabError(PlayFabError error) { Debug.Log(error.GenerateErrorReport()); }
    void DisplayError(string error) { Debug.LogError(error); }
    public List<PlayFab.ServerModels.FriendInfo> _friends = null;
    public void GetFriends()
    {
        PlayFabServerAPI.GetFriendsList(new PlayFab.ServerModels.GetFriendsListRequest
        {
            PlayFabId = MyPlayfabID,
            ProfileConstraints = new PlayFab.ServerModels.PlayerProfileViewConstraints
            {
                ShowStatistics = true,
                ShowTags = true
            },
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result => {
            _friends = result.Friends;
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }
    public List<PlayFab.ClientModels.PlayerLeaderboardEntry> rankList;
    public void RequestLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new PlayFab.ClientModels.GetLeaderboardRequest
        {
            StatisticName = "cup",
            StartPosition = 0,
            MaxResultsCount = 10
        }, result =>
        {
            rankList = result.Leaderboard;
            DisplayRank(rankList);
        }, DisplayPlayFabError);
    }
    public void RequestLeaderboardForYou()
    {
        PlayFabClientAPI.GetLeaderboard(new PlayFab.ClientModels.GetLeaderboardRequest
        {
            StatisticName = "cup",
            StartPosition = 0,
            MaxResultsCount = 100,
        }, result =>
        {
            DisplayYourRank(result.Leaderboard);
        }, DisplayPlayFabError);
    }
    public Transform rankContainer;
    public RankListItem rankListItem;
    public GameObject rankMenu;
    public RankListItem yourrank;
    void DisplayRank(List<PlayFab.ClientModels.PlayerLeaderboardEntry> rankcache)
    {
        foreach (Transform i in rankContainer)
        {
            Destroy(i.gameObject);
        }
        int rank = 1;
        foreach (var i in rankcache)
        {
            RankListItem item = Instantiate(rankListItem, rankContainer);
            item.playerName.text = i.DisplayName;
            item.cup.text = i.StatValue.ToString();
            item.rank.text = rank.ToString();
            if (rank == 1)
            {
                item.rank1.SetActive(true);
                item.rank2.SetActive(false);
                item.rank3.SetActive(false);
            }
            else if (rank == 2)
            {
                item.rank1.SetActive(false);
                item.rank2.SetActive(true);
                item.rank3.SetActive(false);
            }
            else if (rank == 3)
            {
                item.rank1.SetActive(false);
                item.rank2.SetActive(false);
                item.rank3.SetActive(true);
            }
            else
            {
                item.rank1.SetActive(false);
                item.rank2.SetActive(false);
                item.rank3.SetActive(false);
            }
            if (i.PlayFabId == MyPlayfabID)
            {
                item.rankbg.sprite = item.bgYou;
            }
            else
            {
                item.rankbg.sprite = item.bgNotYou;
            }
            rank++;
        }
    }
    bool isYou;
    void DisplayYourRank(List<PlayFab.ClientModels.PlayerLeaderboardEntry> rankcache)
    {
        int rank = 1;
        foreach (var i in rankcache)
        {
            if (i.PlayFabId == MyPlayfabID)
            {
                RankListItem item = yourrank;
                item.playerName.text = i.DisplayName;
                item.cup.text = i.StatValue.ToString();
                item.rank.text = rank.ToString();
                if (rank == 1)
                {
                    item.rank1.SetActive(true);
                    item.rank2.SetActive(false);
                    item.rank3.SetActive(false);
                }
                else if (rank == 2)
                {
                    item.rank1.SetActive(false);
                    item.rank2.SetActive(true);
                    item.rank3.SetActive(false);
                }
                else if (rank == 3)
                {
                    item.rank1.SetActive(false);
                    item.rank2.SetActive(false);
                    item.rank3.SetActive(true);
                }
                else
                {
                    item.rank1.SetActive(false);
                    item.rank2.SetActive(false);
                    item.rank3.SetActive(false);
                }
                isYou = true;
                break;
            }
            else
            {
                rank++;
            }

        }
        if(isYou == false)
        {
            RankListItem item = yourrank;
            item.playerName.text = playerName;
            item.cup.text = StatusPlayer.Instance.cup.ToString();
            item.rank.text = "?";
        }
    }
    public List<PlayFab.ServerModels.FriendInfo> _friends2 = null;
    public void GetPlayerFriendList(string id, int idAction, string roomName)
    {
        PlayFabServerAPI.GetFriendsList(new PlayFab.ServerModels.GetFriendsListRequest
        {
            PlayFabId = id,
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null
        }, result => {
            _friends2 = result.Friends;
            for (int i = 0; i < _friends2.Count; i++)
            {
                if (_friends2[i].FriendPlayFabId == MyPlayfabID)
                {
                    if (idAction == 0)
                    {
                        List<string> tm = new List<string>();
                        tm.Add("a_friend");
                        SetPlayerFriendTags(id, _friends2[i], tm);
                        Debug.Log("Done");
                        break;
                    }
                    else if (idAction == 1)
                    {
                        RemovePlayerFriend(id, _friends2[i]);
                        Debug.Log("Done");
                        break;
                    }
                    else if (idAction == 2)
                    {
                        if (_friends2[i].Tags != null && _friends2[i].Tags[0] == "request")
                        {
                            List<string> tm = new List<string>();
                            tm.Add("a_friend");
                            SetPlayerFriendTags(id, _friends2[i], tm);
                            Debug.Log("Done");
                            for (int j = 0; i < _friends.Count; i++)
                            {
                                if (_friends[i].FriendPlayFabId == id)
                                {
                                    SetPlayerFriendTags(MyPlayfabID, _friends[i], tm);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                    else if (idAction == 3)
                    {
                        List<string> tm = new List<string>();
                        tm.Add("a_friend");
                        tm.Add("b_invite");
                        tm.Add("c_" + roomName);
                        tm.Add("d_" + PhotonNetwork.CloudRegion);
                        SetPlayerFriendTags(id, _friends2[i], tm);
                        Debug.Log(roomName.Remove(0, 0));
                        break;
                    }
                }
            }
        }, DisplayPlayFabError);
    }
    public void RemoveFriend(PlayFab.ServerModels.FriendInfo friendInfo)
    {
        PlayFabServerAPI.RemoveFriend(new PlayFab.ServerModels.RemoveFriendRequest
        {
            PlayFabId = MyPlayfabID,
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, result => {
            _friends.Remove(friendInfo);
        }, DisplayPlayFabError);
    }
    public void RemovePlayerFriend(string id, PlayFab.ServerModels.FriendInfo friendInfo)
    {
        PlayFabServerAPI.RemoveFriend(new PlayFab.ServerModels.RemoveFriendRequest
        {
            PlayFabId = id,
            FriendPlayFabId = friendInfo.FriendPlayFabId
        }, result => {
            _friends2.Remove(friendInfo);
        }, DisplayPlayFabError);
    }
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        base.OnLobbyStatisticsUpdate(lobbyStatistics);
    }
    //public void SetFriendTags(PlayFab.ClientModels.FriendInfo friend, List<string> newTags)
    //{
    //    // update the tags with the edited list
    //    PlayFabClientAPI.SetFriendTags(new PlayFab.ClientModels.SetFriendTagsRequest
    //    {
    //        FriendPlayFabId = friend.FriendPlayFabId,
    //        Tags = newTags
    //    }, tagresult => {
    //        // Make sure to save new tags locally. That way you do not have to hard-update friendlist
    //        friend.Tags = newTags;
    //    }, DisplayPlayFabError);
    //}
    public void SetPlayerFriendTags(string id, PlayFab.ServerModels.FriendInfo friend, List<string> newTags)
    {
        // update the tags with the edited list
        PlayFabServerAPI.SetFriendTags(new PlayFab.ServerModels.SetFriendTagsRequest
        {
            PlayFabId = id,
            FriendPlayFabId = friend.FriendPlayFabId,
            Tags = newTags
        }, tagresult => {
            // Make sure to save new tags locally. That way you do not have to hard-update friendlist
            friend.Tags = newTags;
        }, DisplayPlayFabError);
    }
    public IEnumerator waitToDone(string id)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < Launcher2.Instance._friends.Count; i++)
        {
            if (Launcher2.Instance._friends[i].FriendPlayFabId == id)
            {
                List<string> tm = new List<string>();
                tm.Add("request");
                Launcher2.Instance.SetPlayerFriendTags(Launcher2.Instance.MyPlayfabID, Launcher2.Instance._friends[i], tm);
                Debug.Log("Done request");
                break;
            }
        }
    }
    public void AddFriendPlayFab(string id, string friendId)
    {
        var request = new PlayFab.ServerModels.AddFriendRequest();
        request.FriendPlayFabId = friendId;
        request.PlayFabId = id;
        PlayFabServerAPI.AddFriend(request, result =>
        {
            Debug.Log("Friend added successfully!");
        }, DisplayPlayFabError);
    }
    [PunRPC]
    public void IncreaseOrDecreaseNumberPlayerReady(bool isIncrease, int i, string id)
    {
        if (isIncrease == true)
        {
            numberPlayerReady++;
            Debug.Log("SentNumberPlayerReadyCmd : " + numberPlayerReady);
        }
        else
        {
            numberPlayerReady--;
        }
        int tm = 0;
        foreach (Transform child in playerListContent)
        {
            if (tm == i)
            {
                child.GetComponent<PlayerListItem2>().isReady = isIncrease;
                child.GetComponent<PlayerListItem2>().idPlayer = id;
                break;
            }
            tm++;
        }
    }
    [PunRPC]
    public void SetUpPlayerListItemCmd(int i, string id)
    {
        int tm = 0;
        foreach (Transform child in playerListContent)
        {
            if (tm == i)
            {
                child.GetComponent<PlayerListItem2>().idPlayer = id;
                break;
            }
            tm++;
        }
    }
    public GameObject buttonReadyOrNot;
    public GameObject readyText;
    public GameObject unreadyText;
    public void ReadyOrNot()
    {
        int tm = 0;
        foreach (Transform child in playerListContent)
        {
            if (child.GetComponent<PlayerListItem2>().player.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                child.GetComponent<PlayerListItem2>().isReady = isReady;
            }
        }
        if (isReady == true)
        {
            isReady = false;
            readyText.SetActive(true);
            unreadyText.SetActive(false);
        }
        else
        {
            isReady = true;
            readyText.SetActive(false);
            unreadyText.SetActive(true);
        }
        foreach (Transform child in playerListContent)
        {
            if (child.GetComponent<PlayerListItem2>().player.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                photonView.RPC("IncreaseOrDecreaseNumberPlayerReady", RpcTarget.AllBuffered, isReady, tm, MyPlayfabID);
                break;
            }
            tm++;
        }
    }
    public void GetDataForPlayListItem()
    {
        int tm = 0;
        foreach (Transform child in playerListContent)
        {
            if (child.GetComponent<PlayerListItem2>().player.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                child.GetComponent<PlayerListItem2>().isReady = isReady;
            }
        }
        if (isReady == true)
        {
            readyText.SetActive(false);
            unreadyText.SetActive(true);
        }
        else
        {
            readyText.SetActive(true);
            unreadyText.SetActive(false);
        }
        foreach (Transform child in playerListContent)
        {
            if (child.GetComponent<PlayerListItem2>().player.IsLocal)
            {
                photonView.RPC("IncreaseOrDecreaseNumberPlayerReady", RpcTarget.AllBuffered, isReady, tm, MyPlayfabID);
                break;
            }
            tm++;
        }
    }
    public void SentNumberPlayerReady()
    {
        photonView.RPC("SentNumberPlayerReadyCmd", RpcTarget.AllBuffered, numberPlayerReady);
    }
    [PunRPC]
    void SentNumberPlayerReadyCmd(int num)
    {
        numberPlayerReady = num;
    }
    public void GetDataForPlayListItemAgain()
    {
        int tm = 0;
        foreach (Transform child in playerListContent)
        {
            if (child.GetComponent<PlayerListItem2>().player.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                photonView.RPC("SetUpPlayerListItemCmd", RpcTarget.AllBuffered, tm, MyPlayfabID);
                break;
            }
            tm++;
        }
    }
    public Toggle isPrivate;
    public Toggle player6;
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        if (isPrivate.isOn == true)
        {
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = false;
        }
        else
        {
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
        }
        roomOptions.MaxPlayers = 10;
        Debug.Log(isPrivate.isOn);
        PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
        createRoomMenu.SetActive(false);
        LoadingMenu.SetActive(true);
    }
    public GameObject roomPlay;
    public override void OnJoinedRoom()
    {
        //MenuManager2.Instance.OpenMenu("room");
        roomPlay.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.CurrentRoom.Name.Contains("_mergeRoom"))
        {
            MergeRoomMenu.SetActive(true);
            isMerge = true;
        }
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        numberPlayerReady = 0;
        for (int i = 0; i < players.Count(); i++)
        {
            PlayerListItem2 tm = Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem2>();
            tm.SetUp(players[i], true);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        buttonReadyOrNot.SetActive(!PhotonNetwork.IsMasterClient);
        if (isReady == true)
        {
            readyText.SetActive(false);
            unreadyText.SetActive(true);
        }
        else
        {
            readyText.SetActive(false);
            unreadyText.SetActive(true);
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        buttonReadyOrNot.SetActive(!PhotonNetwork.IsMasterClient);
        if (isReady == true)
        {
            readyText.SetActive(false);
            unreadyText.SetActive(true);
        }
        else
        {
            readyText.SetActive(false);
            unreadyText.SetActive(true);
        }
        if (newMasterClient.IsLocal)
        {
            if (isReady == false)
            {
                ReadyOrNot();
            }
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        Debug.LogError("Room Creation Failed: " + message);
        MenuManager2.Instance.OpenMenu("error");
    }
    public GameObject loadingToMap;
    public bool isMerge;
    public GameObject MergeRoomMenu;
    public TMPro.TextMeshProUGUI mergeRoomText;
    public void StartGame()
    {
        if (test == false)
        {
            Debug.Log(PhotonNetwork.PlayerList.Length);
            if (numberPlayerReady >= PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.CurrentRoom.IsVisible == true && PhotonNetwork.CurrentRoom.PlayerCount < 10)
                {
                    MergeRoom();
                }
                else
                {
                    if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                    {
                        isStart = true;
                        photonView.RPC("AddWhenLoadLevel", RpcTarget.AllBuffered);
                        photonView.RPC("NeedBot", RpcTarget.AllBuffered, false);
                        fadeScene.PhotonLevelLoad("mapgenerate");
                        PhotonNetwork.CurrentRoom.IsOpen = false;
                    }
                    else
                    {
                        AlertPopup("NEED MORE THAN ONE PLAYER TO START THE GAME !");
                    }
                }
            }
        }
        else
        {
            isStart = true;
            photonView.RPC("AddWhenLoadLevel", RpcTarget.AllBuffered);
            if (!testWithBot)
            {
                photonView.RPC("NeedBot", RpcTarget.AllBuffered, false);
            }
            else
            {
                photonView.RPC("NeedBot", RpcTarget.AllBuffered, true);
            }
            fadeScene.PhotonLevelLoad("mapgenerate");
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }
    void MergeRoom()
    {
        photonView.RPC("IsMerging2", RpcTarget.AllBuffered);
        if (roomMerge != null)
        {
            foreach (RoomInfo item in roomMerge)
            {
                if (item.IsVisible == true && (item.PlayerCount + PhotonNetwork.CurrentRoom.PlayerCount) <= 10)
                {
                    Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
                    photonView.RPC("JoinNewRoom2", RpcTarget.AllBuffered, item.Name);
                    foreach (Photon.Realtime.Player player in players)
                    {
                        PhotonNetwork.CloseConnection(player);
                    }
                    photonView.RPC("IsMerging", RpcTarget.AllBuffered);
                    break;
                }
            }
        }
        if (isMerge == false)
        {
            StartCoroutine(MakeMergeRoom());
        }
    }
    [PunRPC]
    void IsMerging()
    {
        isMerge = true;
    }
    [PunRPC]
    void IsMerging2()
    {
        MergeRoomMenu.SetActive(true);
    }
    IEnumerator MakeMergeRoom()
    {
        string tm = PhotonNetwork.CurrentRoom.Name;
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        photonView.RPC("IsMerging", RpcTarget.AllBuffered);
        photonView.RPC("JoinNewRoom", RpcTarget.AllBuffered, tm);
        foreach (Photon.Realtime.Player player in players)
        {
            PhotonNetwork.CloseConnection(player);
        }
        float randomTime = Random.RandomRange(20f, 29f);
        yield return new WaitForSeconds(randomTime);
        mergeRoomText.text = "10";
        yield return new WaitForSeconds(1f);
        isStart = true;
        photonView.RPC("AddWhenLoadLevel", RpcTarget.AllBuffered);
        photonView.RPC("NeedBot", RpcTarget.AllBuffered, true);
        fadeScene.PhotonLevelLoad("mapgenerate");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }
    [PunRPC]
    void JoinNewRoom(string tm)
    {
        StartCoroutine(JoinNewRoomWait(tm));
    }

    IEnumerator JoinNewRoomWait(string tm)
    {
        yield return new WaitForSeconds(3f);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(tm + "_mergeRoom", roomOptions, null, null);
    }
    [PunRPC]
    void JoinNewRoom2(string tm)
    {
        StartCoroutine(JoinNewRoomWait2(tm));
    }

    IEnumerator JoinNewRoomWait2(string tm)
    {
        yield return new WaitForSeconds(3f);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(tm, roomOptions, null, null);
    }
    public IEnumerator OnFadeCompletePhoton()
    {
        yield return new WaitForSeconds(0.8f);
        loadingToMap.SetActive(true);
    }
    [PunRPC]
    void AddWhenLoadLevel()
    {        
        isMerge = false;
        buttonStart.SetActive(false);
        //StatusPlayer.Instance.ServerGetTitleData();
        fadeScene.FadeToLevelPhoton();
        StartCoroutine(OnFadeCompletePhoton());
        InventoryManager.inventory.SaveTimeBeforeStartGame();
        //RoomManager2.Instance.numberPlayerCup = numberCups;
    }
    public GameObject LoadingMenu;
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        numberPlayerReady = 0;
        Debug.Log("Left room");
        for (int i = 0; i < players.Count(); i++)
        {
            PlayerListItem2 tm = Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem2>();
            tm.SetUp(players[i], true);
        }
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        //MenuManager2.Instance.OpenMenu("loading");
        changeName.SetActive(false);
        CodeRoom.SetActive(false);
    }
    public void JoinRoomByName(string name)
    {
        PhotonNetwork.JoinRoom(name);
        StartCoroutine(waitToFail());
    }
    public TMP_InputField CodeInputField;
    string roomCode;
    public void JoinRoomByCode()
    {
        roomCode = CodeInputField.text;
        PhotonNetwork.JoinRoom(roomCode);
        CodeRoom.SetActive(false);
        StartCoroutine(waitToFail());
    }
    public GameObject roomMenu;
    IEnumerator waitToFail()
    {
        yield return new WaitForSeconds(2f);
        if (!roomMenu.activeSelf)
        {
            Debug.Log("Can't find room");
            title.SetActive(true);
        }
    }
    public override void OnLeftRoom()
    {
        isCreate = false;
        title.SetActive(true);
    }
    List<RoomInfo> roomMerge = new List<RoomInfo>();
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomMerge.Clear();
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            if (roomList[i].Name.Contains("_mergeRoom") && roomList[i].IsVisible == true)
            {
                Debug.Log(roomList[i].Name);
                roomMerge.Add(roomList[i]);
            }

        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerListItem2 tm = Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem2>();
        tm.SetUp(newPlayer, true);
    }
    public void ChangeName()
    {
        if (StatusPlayer.Instance.levelPlayer >= ConfigRequireLevel.levelChangeName)
        {
            if (string.IsNullOrEmpty(NameInputField.text))
            {
                AlertPopup("PLEASE FILL YOUR NAME");
            }
            else if (NameInputField.text.Length > 10)
            {
                AlertPopup("MAXIMUM NAME LENGTH IS 10.");
            }
            else
            {
                ChangeName(NameInputField.text);
            }
        }
        else
        {
            string tm = "YOU NEED TO LEVEL+ " + ConfigRequireLevel.levelChangeName.ToString() + " !!!";
            AlertPopup(tm);
        }

    }
    public void QuickJoin()
    {
        if (InventoryManager.inventory.energy >= ConfigEnergyShop.energyCost)
        {
            if (StatusPlayer.Instance.levelPlayer >= ConfigRequireLevel.levelPlayWithFriends)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                StartCoroutine(waitToPlayWithBot());
            }
        }
        else
        {
            popupBuyEnergy.SetActive(true);
        }
    }
    IEnumerator waitToPlayWithBot()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(Random.Range(0, 99999).ToString("00000") + "bot", roomOptions);
        yield return new WaitUntil(HadCreatedRoom);
        isStart = true;
        photonView.RPC("AddWhenLoadLevel", RpcTarget.AllBuffered);
        photonView.RPC("NeedBot", RpcTarget.AllBuffered, true);
        fadeScene.PhotonLevelLoad("mapgenerate");
    }
    bool HadCreatedRoom()
    {
        return PhotonNetwork.CurrentRoom != null;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No room have create");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(Random.Range(0, 99999).ToString("00000"), roomOptions);
        isCreate = true;
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {

    }
    public GameObject CodeRoom;
    public GameObject title;
    public GameObject settingMenu;
    public GameObject createRoomMenu;
    public void OpenCreateRoom()
    {
        if (StatusPlayer.Instance.levelPlayer >= ConfigRequireLevel.levelPlayWithFriends)
        {
            if (InventoryManager.inventory.energy >= ConfigEnergyShop.energyCost)
            {
                createRoomMenu.SetActive(true);
            }
            else
            {
                popupBuyEnergy.SetActive(true);
            }
        }
        else
        {
            string tm = "YOU NEED TO LEVEL+ " + ConfigRequireLevel.levelPlayWithFriends.ToString() + " !!!";
            AlertPopup(tm);
        }

    }
    public void OpenRankMenu()
    {
        rankMenu.SetActive(true);
    }
    //public void CloseCreateRoom()
    //{
    //    StartCoroutine(CloseAndOpenMenu(createRoomMenu, title));
    //}
    public void FindRoomWithCode()
    {
        if (StatusPlayer.Instance.levelPlayer >= ConfigRequireLevel.levelPlayWithFriends)
        {
            if (InventoryManager.inventory.energy >= ConfigEnergyShop.energyCost)
            {
                //StartCoroutine(CloseAndOpenMenu(title, CodeRoom));
                CodeRoom.SetActive(true);
            }
            else
            {
                popupBuyEnergy.SetActive(true);
            }
        }
        else
        {
            string tm = "YOU NEED TO LEVEL+ " + ConfigRequireLevel.levelPlayWithFriends.ToString() + " !!!";
            AlertPopup(tm);
        }

    }
    public void SettingMenu()
    {
        settingMenu.SetActive(true);
    }
    public GameObject charMenu;
    public void OpenCharMenu()
    {
        charMenu.SetActive(true);
    }

    public GameObject Code;
    public TMP_InputField GiftInputField;
    public void OpenCodeMenu()
    {
        Code.SetActive(true);
    }
    //public void GetCode()
    //{
    //    if (GiftInputField.text == "admin")
    //    {
    //        StatusPlayer.Instance.Money += 20000;
    //        InventoryManager.inventory.UpdateMoney();
    //        StatusPlayer.Instance.levelPlayer += 5;
    //        StatusPlayer.Instance.UpdateDataPlayer(StatusPlayer.Instance.levelPlayer, "levelPlayer");
    //        InventoryManager.inventory.UpdateExp();
    //        test = true;
    //        Code.SetActive(false);
    //    }
    //    else if (GiftInputField.text == "off_test_mode")
    //    {
    //        test = false;
    //        Code.SetActive(false);
    //    }
    //    else if (GiftInputField.text == "off_test_bot")
    //    {
    //        testWithBot = false;
    //        Code.SetActive(false);
    //    }
    //    else if (GiftInputField.text == "on_test_mode")
    //    {
    //        test = true;
    //        Code.SetActive(false);
    //    }
    //    else if (GiftInputField.text == "on_test_bot")
    //    {
    //        testWithBot = true;
    //        Code.SetActive(false);
    //    }
    //    else
    //    {
    //        AlertPopup("WRONG CODE !!!");
    //        Code.SetActive(false);
    //        test = false;
    //    }
    //}
    public GameObject friendMenu;
    public void OpenFriendMenu()
    {
        friendMenu.SetActive(true);
    }

    //IEnumerator CloseAndOpenMenu(GameObject closeMenu, GameObject openMenu)
    //{
    //    if (closeMenu != null)
    //    {
    //        closeMenu.GetComponent<Animator>().Play("moveleft");
    //    }
    //    yield return new WaitForSeconds(0f);
    //    if (closeMenu != null)
    //    {
    //        closeMenu.SetActive(false);
    //    }
    //    if (openMenu != null)
    //    {
    //        openMenu.SetActive(true);
    //        if (openMenu == title)
    //        {
    //            openMenu.GetComponent<Animator>().Play("notmovetitlemenu");
    //        }
    //        else
    //        {
    //            openMenu.GetComponent<Animator>().Play("notmove");
    //        }
    //    }
    //    friendMenu.SetActive(true);
    //}
    public string MyPlayfabID;
    public TMPro.TextMeshProUGUI MyPlayfabIDtext;
    //public int numberCups;


    public void GetAccountInfo()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, Successs, fail);
    }
    void Successs(GetAccountInfoResult result)
    {
        MyPlayfabID = result.AccountInfo.PlayFabId;
        StatusPlayer.Instance.MyPlayfabID = MyPlayfabID;
        MyPlayfabIDtext.text = MyPlayfabID;
        GetPlayerProfile(MyPlayfabID);
    }

    public void GetPlayerProfile(string playFabId)
    {
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
            int numberMatch = 0;
            int numberMatchFinish = 0;
            int numberMatchWin = 0;
            int numberSeekWin = 0;
            int numberHideWin = 0;
            //Get Name
            if (string.IsNullOrEmpty(result.PlayerProfile.DisplayName))
            {
                RandomName();
            }
            else
            {
                playerName = result.PlayerProfile.DisplayName;
                playerNameText.text = playerName;
                PhotonNetwork.NickName = playerName;
            }

            bool haveCup = false, haveNumberMatch = false, haveNumberMatchFinished = false,
            haveNumberMatchWin = false, haveHideWin = false, haveSeekWin = false, haveLevel = false,
            haveMoney = false, haveRemoveAd = false, haveNumberLosing = false, haveDailyPack = false;
            foreach (var i in result.PlayerProfile.Statistics)
            {
                //Profile player
                switch (i.Name)
                {
                    case "cup":
                        {
                            StatusPlayer.Instance.cup = i.Value;
                            haveCup = true;
                            break;
                        }
                    case "Money":
                        {
                            StatusPlayer.Instance.Money = i.Value;
                            haveMoney = true;
                            break;
                        }
                    case "levelPlayer":
                        {
                            StatusPlayer.Instance.levelPlayer = i.Value;
                            PlayerPrefs.SetInt("LevelPlayer", i.Value);
                            haveLevel = true;
                            break;
                        }
                    case "numberMatch":
                        {
                            numberMatch = i.Value;
                            haveNumberMatch = true;
                            break;
                        }
                    case "numberMatchFinish":
                        {
                            numberMatchFinish = i.Value;
                            haveNumberMatchFinished = true;
                            break;
                        }
                    case "numberMatchWin":
                        {
                            numberMatchWin = i.Value;
                            haveNumberMatchWin = true;
                            break;
                        }
                    case "numberHideWin":
                        {
                            numberHideWin = i.Value;
                            haveHideWin = true;
                            break;
                        }
                    case "numberSeekWin":
                        {
                            numberSeekWin = i.Value;
                            haveSeekWin = true;
                            break;
                        }
                    case "isRemoveAd":
                        {
                            StatusPlayer.Instance.isRemoveAd = i.Value;
                            haveRemoveAd = true;
                            break;
                        }
                    case "losing":
                        {
                            StatusPlayer.Instance.losing = i.Value;
                            haveNumberLosing = true;
                            break;
                        }
                    case "dailyPack":
                        {
                            StatusPlayer.Instance.dailyPack = i.Value;
                            haveDailyPack = true;
                            break;
                        }
                }
            }

            //When no player data found
            //Add default value or from playerPrefabs
            if (haveCup == false)
            {
                StatusPlayer.Instance.cup = 0;
            }

            if (haveNumberMatch == false)
            {
                numberMatch = 0;
            }
            if (haveNumberMatchFinished == false)
            {
                numberMatchFinish = 0;
            }
            if (haveNumberMatchWin == false)
            {
                numberMatchWin = 0;
            }
            if (haveHideWin == false)
            {
                numberHideWin = 0;
            }
            if (haveSeekWin == false)
            {
                numberSeekWin = 0;
            }
            if (haveLevel == false)
            {
                if (PlayerPrefs.GetInt("LevelPlayer") == 0)
                {
                    StatusPlayer.Instance.levelPlayer = 1;
                }
                else
                {
                    StatusPlayer.Instance.levelPlayer = PlayerPrefs.GetInt("LevelPlayer");
                }
            }
            if (haveMoney == false)
            {
                if (PlayerPrefs.GetInt("money") == 0)
                {
                    StatusPlayer.Instance.levelPlayer = 0;
                }
                else
                {
                    StatusPlayer.Instance.levelPlayer = PlayerPrefs.GetInt("money");
                }
            }
            if (haveRemoveAd == false)
            {
                StatusPlayer.Instance.isRemoveAd = 0;
                StatusPlayer.Instance.UpdateDataPlayer(0, "isRemoveAd");
            }
            if (haveDailyPack == false)
            {
                StatusPlayer.Instance.dailyPack = 3;
            }
            //Update data to player status and menu
            StatusPlayer.Instance.GetDataPlayer(numberMatch, numberMatchFinish, numberMatchWin, numberHideWin, numberSeekWin);
            InventoryManager.inventory.cupText.text = StatusPlayer.Instance.cup.ToString();
            InventoryManager.inventory.UpdateExp();
            InventoryManager.inventory.AddText();
            StatusPlayer.Instance.GetUserLastTime(MyPlayfabID);

            if (!haveCup || !haveLevel || !haveMoney || !haveNumberMatch || !haveNumberMatchFinished || !haveNumberMatchWin || !haveHideWin
            || !haveSeekWin || !haveNumberLosing)
            StatusPlayer.Instance.UpdateMultiDataPlayer(StatusPlayer.Instance.cup, 1, StatusPlayer.Instance.Money,StatusPlayer.Instance.levelPlayer
                , numberMatch, numberMatchFinish, numberMatchWin, numberHideWin, numberSeekWin, ConvertRegionToId(PhotonNetwork.CloudRegion), 0, 0 , 3);
        },        
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    private void OnStatisticsUpdated(PlayFab.ClientModels.UpdatePlayerStatisticsResult updateResult)
    {
        Debug.Log("Successfully submitted high score");
    }

    private void FailureCallback(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }


    public void ChangeDisplayName(string nameChange)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameChange
        }, result => {
            playerName = result.DisplayName;
            playerNameText.text = result.DisplayName;
            PhotonNetwork.NickName = playerName;
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }
    void RandomName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = "guest" + Random.Range(0, 999999).ToString("000000")
        }, result => {
            playerName = result.DisplayName;
            playerNameText.text = result.DisplayName;
            PhotonNetwork.NickName = Launcher2.Instance.playerName;
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }
    void ChangeName(string name)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        }, result => {
            playerName = result.DisplayName;
            playerNameText.text = result.DisplayName;
            PhotonNetwork.NickName = playerName;
            PhotonNetwork.NickName = NameInputField.text;
            AlertPopup("CHANGE NAME SUCCESSFULLY");
            changeName.SetActive(false);
        },
        error => AlertPopup(error.ErrorMessage)
        );
    }
    void fail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new PlayFab.ClientModels.GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetStatistics(PlayFab.ClientModels.GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
    }
    public GameObject popupAlert;
    public TMPro.TextMeshProUGUI alertText;
    public void AlertPopup(string alertMessage)
    {
        popupAlert.SetActive(true);
        popupAlert.GetComponent<Animator>().SetBool("fade", false);
        alertText.text = alertMessage;
        StartCoroutine(AlertPopupFade());
    }
    IEnumerator AlertPopupFade()
    {
        yield return new WaitForSeconds(0.1f);
        popupAlert.GetComponent<Animator>().SetBool("fade", true);
        yield return new WaitForSeconds(1.1f);
        popupAlert.SetActive(false);
    }
    public void TutorialScene1()
    {
        buttonStart.SetActive(false);
        fadeScene.FadeToLevel("Scene1");
    }
    public void TutorialScene2()
    {
        buttonStart.SetActive(false);
        fadeScene.FadeToLevel("Scene2");
    }
    public void TutorialScene3()
    {
        buttonStart.SetActive(false);
        fadeScene.FadeToLevel("Scene3");
    }
    public int ConvertRegionToId(string regionToken)
    {
        int i = 0;
        switch (regionToken)
        {
            case "asia":
                i = 0;
                break;
            case "au":
                i = 1;
                break;
            case "cae":
                i = 2;
                break;
            case "cn":
                i = 3;
                break;
            case "eu":
                i = 4;
                break;
            case "in":
                i = 5;
                break;
            case "jp":
                i = 6;
                break;
            case "ru":
                i = 7;
                break;
            case "rue":
                i = 8;
                break;
            case "za":
                i = 9;
                break;
            case "sa":
                i = 10;
                break;
            case "kr":
                i = 11;
                break;
            case "us":
                i = 12;
                break;
            case "usw":
                i = 13;
                break;
            default:
                break;
        }
        return i;
    }
    public string ConvertIdToRegion(int id)
    {
        string i = "";
        switch (id)
        {
            case 0:
                i = "asia";
                break;
            case 1:
                i = "au";
                break;
            case 2:
                i = "cae";
                break;
            case 3:
                i = "cn";
                break;
            case 4:
                i = "eu";
                break;
            case 5:
                i = "in";
                break;
            case 6:
                i = "jp";
                break;
            case 7:
                i = "ru";
                break;
            case 8:
                i = "rue";
                break;
            case 9:
                i = "za";
                break;
            case 10:
                i = "sa";
                break;
            case 11:
                i = "kr";
                break;
            case 12:
                i = "us";
                break;
            case 13:
                i = "usw";
                break;
            default:
                i = "asia";
                break;
        }
        return i;
    }
    public GameObject missonDaily;
    public void OpenMissionMenu()
    {
        missonDaily.SetActive(true);
    }
    public GameObject ModeMenu;
    public void OpenModeMenu()
    {
        ModeMenu.SetActive(true);
    }


    public TMPro.TextMeshProUGUI goldDailyText;
    public GameObject dailyreward;
    public GameObject chestClick;
    public GameObject chest;
    public void RandomGoldDaily()
    {
        chestClick.SetActive(true);
        int i = Random.Range(1, 1000);
        goldDailyText.text = i.ToString();
        StatusPlayer.Instance.Money += i;
        InventoryManager.inventory.UpdateMoney();
    }
    public void ClickChest()
    {
        dailyreward.SetActive(true);
        chest.SetActive(false);
    }
}